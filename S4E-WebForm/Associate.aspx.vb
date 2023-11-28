Imports System.Data.SqlClient
Imports System.Web.UI.WebControls.Expressions
Imports Newtonsoft.Json.Linq
Imports S4E_WebForm
Imports System.Web.Services
Imports System.Globalization
Imports System.ComponentModel

Public Class About
    Inherits System.Web.UI.Page
    Private connectionString As String = ConfigurationManager.ConnectionStrings("S4E_Challenge").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim assocId As Integer = GetAssociateId()
        GetAvailableCompanies(assocId)
        GetSelectedCompanies(assocId)
        GetAssociates()

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnInsert.Click

        Dim dataLayer As New DataLayer()
        Dim associate As New Associate()
        Dim companies As List(Of Company) = GetCompanies()

        associate.Name = txtName.Text
        associate.Cpf = txtCpf.Text
        associate.BirthDate = DateTime.ParseExact(txtBirth.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date
        associate.Companies = companies

        dataLayer.AddAssociate(associate)
        GetAssociates()
        ResetFields()

    End Sub

    Private Sub GetAvailableCompanies(assocId As Integer)
        Dim query As String = "SELECT COMP.ID, C_NAME FROM T_COMP COMP LEFT JOIN T_REL REL ON COMP.ID = REL.C_ID AND REL.A_ID = @AssocId WHERE REL.ID IS NULL"

        Dim SelectedValues As New List(Of String)
        For Each item As ListItem In ListBox2.Items
            SelectedValues.Add(item.Value)
        Next

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@AssocId", assocId)
                Dim reader As SqlDataReader = command.ExecuteReader()

                While reader.Read()
                    Dim id As Integer = Convert.ToInt32(reader("ID"))
                    Dim name As String = reader("C_NAME").ToString()

                    If Not SelectedValues.Contains(id.ToString()) Then
                        If Not ListBox1.Items.FindByValue(id.ToString()) IsNot Nothing Then
                            ListBox1.Items.Add(New ListItem(name, id.ToString()))
                        End If
                    End If
                End While

                reader.Close()
            End Using
        End Using
    End Sub


    Private Sub GetSelectedCompanies(assocId As Integer)
        Dim query As String = "SELECT COMP.ID, C_NAME FROM T_COMP COMP INNER JOIN T_REL REL ON COMP.ID = REL.C_ID WHERE REL.A_ID = @AssocId"

        Dim SelectedValues As New List(Of String)
        For Each item As ListItem In ListBox1.Items
            SelectedValues.Add(item.Value)
        Next

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@AssocId", assocId)
                Dim reader As SqlDataReader = command.ExecuteReader()

                While reader.Read()
                    Dim id As Integer = Convert.ToInt32(reader("ID"))
                    Dim name As String = reader("C_NAME").ToString()

                    If Not SelectedValues.Contains(id.ToString()) Then
                        If Not ListBox2.Items.FindByValue(id.ToString()) IsNot Nothing Then
                            ListBox2.Items.Add(New ListItem(name, id.ToString()))
                        End If
                    End If
                End While


                reader.Close()
            End Using
            connection.Close()
        End Using
    End Sub
    Private Function GetCompanies() As List(Of Company)
        Dim companies As New List(Of Company)
        Dim idList As New List(Of Integer)
        For Each item As ListItem In ListBox2.Items
            idList.Add(item.Value)
        Next
        If idList.Count > 0 Then
            Dim query As String = "SELECT * FROM T_COMP WHERE ID IN (" + String.Join(",", idList) + ")"

            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Using command As New SqlCommand(query, connection)
                    Dim reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim company As New Company()
                        company.Id = Convert.ToInt32(reader("ID"))
                        company.Name = Convert.ToString(reader("C_NAME"))
                        company.Cnpj = Convert.ToString(reader("C_CNPJ"))
                        companies.Add(company)
                    End While
                End Using
            End Using
        End If
        Return companies
    End Function

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        For i As Integer = ListBox1.Items.Count - 1 To 0 Step -1
            If ListBox1.Items(i).Selected Then
                Dim newItem As New ListItem(ListBox1.Items(i).Text, ListBox1.Items(i).Value)
                ListBox2.Items.Add(newItem)
                ListBox1.Items.RemoveAt(i)
            End If
        Next
    End Sub
    Protected Sub btnRemove_Click(sender As Object, e As EventArgs)
        For i As Integer = ListBox2.Items.Count - 1 To 0 Step -1
            If ListBox2.Items(i).Selected Then
                Dim newItem As New ListItem(ListBox2.Items(i).Text, ListBox2.Items(i).Value)
                ListBox1.Items.Add(newItem)
                ListBox2.Items.RemoveAt(i)
            End If
        Next
    End Sub
    Private Sub GetAssociates()
        Dim query As String = "SELECT A.Id, A.A_Name AS Nome, A.A_CPF AS CPF, A.A_BIRTH AS 'Data de Nascimento', " &
                              "Empresas = STUFF((SELECT ', ' + C.C_NAME " &
                              "FROM T_REL R INNER JOIN T_COMP C ON R.C_ID = C.ID " &
                              "WHERE R.A_ID = A.Id FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') " &
                              "FROM T_ASSOC A;"
        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand(query, connection)
                Dim sd As New SqlDataAdapter(command)
                Dim dt As New DataTable
                sd.Fill(dt)
                gridViewAssoc.DataSource = dt
                gridViewAssoc.DataBind()
            End Using
        End Using
    End Sub

    Protected Sub gridViewAssoc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gridViewAssoc.SelectedIndexChanged
        Dim assocId As Integer = Convert.ToInt32(gridViewAssoc.SelectedRow.Cells(1).Text)

        txtName.Text = HttpUtility.HtmlDecode(gridViewAssoc.SelectedRow.Cells.Item(2).Text.ToString)
        txtCpf.Text = HttpUtility.HtmlDecode(gridViewAssoc.SelectedRow.Cells.Item(3).Text.ToString)
        txtBirth.Text = HttpUtility.HtmlDecode(gridViewAssoc.SelectedRow.Cells.Item(4).Text.ToString)
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        GetAvailableCompanies(assocId)
        GetSelectedCompanies(assocId)
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim assocId As Integer = GetAssociateId()
        Dim dataLayer As New DataLayer()
        Dim associate As New Associate()
        Dim companies As List(Of Company) = GetCompanies()

        associate.Id = assocId
        associate.Name = txtName.Text
        associate.Cpf = txtCpf.Text
        associate.BirthDate = DateTime.Parse(txtBirth.Text).Date
        associate.Companies = companies

        dataLayer.UpdateAssociate(associate)
        GetAssociates()
        ResetFields()
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim dataLayer As New DataLayer()
        Dim associateId As Integer = GetAssociateId()

        dataLayer.DeleteAssociate(associateId)
        GetAssociates()
        ResetFields()
    End Sub
    Private Sub ResetFields()
        Dim assocId As Integer = 0
        txtName.Text = String.Empty
        txtCpf.Text = String.Empty
        txtBirth.Text = String.Empty
        txtOtherFilter.Text = String.Empty

        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        GetAvailableCompanies(assocId)
        GetSelectedCompanies(assocId)

        txtName.Focus()
    End Sub

    Protected Sub btnIdFilter_Click(sender As Object, e As EventArgs) Handles btnIdFilter.Click
        Dim queryString As String = "
            SELECT
                A.Id AS Id,
                A.A_NAME AS Nome,
                A.A_CPF AS CPF,
                A.A_BIRTH AS [Data de Nascimento],
                Empresas = STUFF((
                    SELECT ', ' + C.C_NAME
                    FROM T_REL R
                    INNER JOIN T_COMP C ON R.C_ID = C.ID
                    WHERE R.A_ID = A.Id
                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
            FROM
                T_ASSOC A
            WHERE
                A.Id = @id;"

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Using command As New SqlCommand(queryString, connection)
                command.Parameters.AddWithValue("@id", txtIdFilter.Text)
                Dim sd As New SqlDataAdapter(command)
                Dim dt As New DataTable
                sd.Fill(dt)
                gridViewAssoc.DataSource = dt
                gridViewAssoc.DataBind()
            End Using
        End Using
    End Sub
    Private Function GetAssociateId() As Integer
        Dim assocId As Integer
        Dim query = "SELECT ID FROM T_ASSOC WHERE A_CPF = @cpf"

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@cpf", txtCpf.Text)
                Dim reader As SqlDataReader = command.ExecuteReader()
                While reader.Read()
                    assocId = Convert.ToInt32(reader("ID"))
                End While
            End Using
            connection.Close()
        End Using
        Return assocId
    End Function

    Protected Sub btnIdFilter1_Click(sender As Object, e As EventArgs) Handles btnIdFilter1.Click
        Dim selectedFilter As String = RadioButtonFilters.SelectedValue
        Dim filterValue As String = txtOtherFilter.Text
        Dim query As String
        If selectedFilter = "name" Then
            query = "SELECT A.Id AS Id, A.A_NAME AS Nome, A.A_CPF AS CPF, A.A_BIRTH AS [Data de Nascimento],
                        Empresas = STUFF((
                            SELECT ', ' + C.C_NAME
                            FROM T_REL R
                            INNER JOIN T_COMP C ON R.C_ID = C.ID
                            WHERE R.A_ID = A.Id
                            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
                        FROM T_ASSOC A
                        WHERE A.A_NAME = @filterValue"
        ElseIf selectedFilter = "cpf" Then
            query = "SELECT A.Id AS Id, A.A_NAME AS Nome, A.A_CPF AS CPF, A.A_BIRTH AS [Data de Nascimento],
                        Empresas = STUFF((
                            SELECT ', ' + C.C_NAME
                            FROM T_REL R
                            INNER JOIN T_COMP C ON R.C_ID = C.ID
                            WHERE R.A_ID = A.Id
                            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
                        FROM T_ASSOC A
                        WHERE A.A_CPF = @filterValue"
        Else
            query = "SELECT A.Id AS Id, A.A_NAME AS Nome, A.A_CPF AS CPF, A.A_BIRTH AS [Data de Nascimento],
                        Empresas = STUFF((
                            SELECT ', ' + C.C_NAME
                            FROM T_REL R
                            INNER JOIN T_COMP C ON R.C_ID = C.ID
                            WHERE R.A_ID = A.Id
                            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
                        FROM T_ASSOC A
                        WHERE A.A_BIRTH = @filterValue"
        End If

        Using connection As New SqlConnection(connectionString)
            connection.Open()
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@filterValue", filterValue)
                Dim sd As New SqlDataAdapter(command)
                Dim dt As New DataTable
                sd.Fill(dt)
                gridViewAssoc.DataSource = dt
                gridViewAssoc.DataBind()
            End Using
            connection.Close()
        End Using
    End Sub
End Class