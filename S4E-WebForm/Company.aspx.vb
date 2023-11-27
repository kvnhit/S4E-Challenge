Imports System.Data.SqlClient
Imports System.Globalization

Public Class Contact
    Inherits Page
    Private connectionString As String = ConfigurationManager.ConnectionStrings("S4E_Challenge").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        GetAvailableAssociates()
        GetSelectedAssociates()
        GetCompanies()
    End Sub

    Protected Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click
        Dim dataLayer As New DataLayer()
        Dim company As New Company()
        Dim associates As List(Of Associate) = GetAssociates()

        company.Id = Integer.Parse(txtId.Text)
        company.Name = txtName.Text
        company.Cnpj = txtCnpj.Text
        company.Associates = associates

        dataLayer.AddCompany(company)
        GetCompanies()
        ResetFields()
    End Sub

    Private Sub GetAvailableAssociates()
        Dim query As String = "SELECT ASSOC.ID, A_NAME FROM T_ASSOC ASSOC LEFT JOIN T_REL REL ON ASSOC.ID = REL.A_ID AND REL.C_ID = @CompanyId WHERE REL.ID IS NULL"

        Dim selectedAssociateIds As New List(Of Integer)
        For Each item As ListItem In ListBox2.Items
            selectedAssociateIds.Add(Convert.ToInt32(item.Value))
        Next

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@CompanyId", txtId.Text)
                Dim reader As SqlDataReader = command.ExecuteReader()

                While reader.Read()
                    Dim id As Integer = Convert.ToInt32(reader("ID"))
                    Dim name As String = reader("A_NAME").ToString()

                    If Not selectedAssociateIds.Contains(id) Then
                        If ListBox1.Items.FindByValue(id.ToString()) Is Nothing Then
                            ListBox1.Items.Add(New ListItem(name, id.ToString()))
                        End If
                    End If
                End While

                reader.Close()
            End Using
        End Using
    End Sub


    Private Sub GetSelectedAssociates()
        Dim query As String = "SELECT ASSOC.ID, A_NAME FROM T_ASSOC ASSOC INNER JOIN T_REL REL ON ASSOC.ID = REL.A_ID WHERE REL.C_ID = @CompanyId"
        Dim selectedAssociateIds As New List(Of Integer)
        For Each item As ListItem In ListBox1.Items
            selectedAssociateIds.Add(Convert.ToInt32(item.Value))
        Next

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@CompanyId", txtId.Text)
                Dim reader As SqlDataReader = command.ExecuteReader()

                While reader.Read()
                    Dim id As Integer = Convert.ToInt32(reader("ID"))
                    Dim name As String = reader("A_NAME").ToString()

                    If Not selectedAssociateIds.Contains(id) Then
                        If ListBox2.Items.FindByValue(id.ToString()) Is Nothing Then
                            ListBox2.Items.Add(New ListItem(name, id.ToString()))
                        End If
                    End If
                End While

                reader.Close()
            End Using
        End Using
    End Sub
    Private Function GetAssociates() As List(Of Associate)
        Dim associates As New List(Of Associate)
        Dim idList As New List(Of Integer)
        For Each item As ListItem In ListBox2.Items
            idList.Add(item.Value)
        Next

        Dim query As String = "SELECT * FROM T_ASSOC WHERE ID IN (" + String.Join(",", idList) + ")"

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand(query, connection)
                Dim reader As SqlDataReader = command.ExecuteReader()
                While reader.Read()
                    Dim associate As New Associate()
                    associate.Id = Convert.ToInt32(reader("ID"))
                    associate.Name = Convert.ToString(reader("A_NAME"))
                    associate.Cpf = Convert.ToString(reader("A_CPF"))
                    associate.BirthDate = Convert.ToString(reader("A_BIRTH"))
                    associates.Add(associate)
                End While
            End Using
        End Using
        Return associates
    End Function

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gridViewComp.SelectedIndexChanged
        txtId.Text = HttpUtility.HtmlDecode(gridViewComp.SelectedRow.Cells.Item(1).Text.ToString)
        txtName.Text = HttpUtility.HtmlDecode(gridViewComp.SelectedRow.Cells.Item(2).Text.ToString)
        txtCnpj.Text = HttpUtility.HtmlDecode(gridViewComp.SelectedRow.Cells.Item(3).Text.ToString)
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        GetAvailableAssociates()
        GetSelectedAssociates()
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        For i As Integer = ListBox1.Items.Count - 1 To 0 Step -1
            If ListBox1.Items(i).Selected Then
                Dim newItem As New ListItem(ListBox1.Items(i).Text, ListBox1.Items(i).Value)
                ListBox2.Items.Add(newItem)
                ListBox1.Items.RemoveAt(i)
            End If
        Next
    End Sub

    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        For i As Integer = ListBox2.Items.Count - 1 To 0 Step -1
            If ListBox2.Items(i).Selected Then
                Dim newItem As New ListItem(ListBox2.Items(i).Text, ListBox2.Items(i).Value)
                ListBox1.Items.Add(newItem)
                ListBox2.Items.RemoveAt(i)
            End If
        Next
    End Sub
    Private Sub ResetFields()
        txtId.Text = String.Empty
        txtName.Text = String.Empty
        txtCnpj.Text = String.Empty

        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        GetAvailableAssociates()
        GetSelectedAssociates()

        txtId.Focus()
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim dataLayer As New DataLayer()
        Dim company As New Company()
        Dim associates As List(Of Associate) = GetAssociates()

        company.Id = Integer.Parse(txtId.Text)
        company.Name = txtName.Text
        company.Cnpj = txtCnpj.Text
        company.Associates = associates

        dataLayer.UpdateCompany(company)
        GetCompanies()
        ResetFields()
    End Sub
    Private Sub GetCompanies()
        Dim query As String = "
            SELECT
                C.ID AS Id,
                C.C_NAME AS Nome,
                C.C_CNPJ AS CNPJ,
                Associados = STUFF((
                    SELECT ', ' + A.A_NAME
                    FROM T_REL R
                    INNER JOIN T_ASSOC A ON R.A_ID = A.ID
                    WHERE R.C_ID = C.ID
                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
            FROM
                T_COMP C; "

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand(query, connection)
                Dim sd As New SqlDataAdapter(command)
                Dim dt As New DataTable
                sd.Fill(dt)
                gridViewComp.DataSource = dt
                gridViewComp.DataBind()
            End Using
        End Using
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim dataLayer As New DataLayer()
        Dim companyId As Integer = txtId.Text

        dataLayer.DeleteCompany(companyId)
        GetCompanies()
        ResetFields()
    End Sub

    Protected Sub btnIdFilter_Click(sender As Object, e As EventArgs) Handles btnIdFilter.Click
        Dim queryString As String = "
            SELECT
                C.ID AS Id,
                C.C_NAME AS Nome,
                C.C_CNPJ AS CNPJ,
                Associados = STUFF((
                    SELECT ', ' + A.A_NAME
                    FROM T_REL R
                    INNER JOIN T_ASSOC A ON R.A_ID = A.Id
                    WHERE R.C_ID = C.ID
                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
            FROM
                T_COMP C
            WHERE
                C.ID = @companyId;"

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Using command As New SqlCommand(queryString, connection)
                command.Parameters.AddWithValue("@companyId", txtIdFilter.Text)
                Dim sd As New SqlDataAdapter(command)
                Dim dt As New DataTable
                sd.Fill(dt)
                gridViewComp.DataSource = dt
                gridViewComp.DataBind()
            End Using
        End Using
    End Sub
End Class