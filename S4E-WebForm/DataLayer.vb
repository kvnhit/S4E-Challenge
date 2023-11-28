Imports System.ComponentModel.Design
Imports System.Configuration
Imports System.Data.SqlClient
Public Class DataLayer
    Private connectionString As String = ConfigurationManager.ConnectionStrings("S4E_Challenge").ConnectionString

    Public Sub AddAssociate(ByVal associate As Associate)
        Dim associateId As Integer = 0

        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim query As New SqlCommand("INSERT INTO T_ASSOC (A_NAME, A_CPF, A_BIRTH) VALUES (@Name, @Cpf, @BirthDate); SELECT SCOPE_IDENTITY()", connection)
                query.Parameters.AddWithValue("@Name", associate.Name)
                query.Parameters.AddWithValue("@Cpf", associate.Cpf)
                query.Parameters.AddWithValue("@BirthDate", associate.BirthDate.Date)

                associateId = Convert.ToInt32(query.ExecuteScalar())

                MsgBox("Insert realizado com sucesso!", MsgBoxStyle.Information, "Message")

                connection.Close()
            End Using

            If associate.Companies.Count > 0 Then
                AddAssociateRelation(associate.Companies, associateId)
            End If

        Catch ex As Exception
            MsgBox("Ocorreu um erro durante a inserção: " & ex.Message, MsgBoxStyle.Exclamation, "Erro")
        End Try

    End Sub

    Public Sub AddCompany(ByVal company As Company)
        Dim companyId As Integer = 0

        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim query As New SqlCommand("INSERT INTO T_COMP VALUES (@Name, @Cnpj); SELECT SCOPE_IDENTITY()", connection)
                query.Parameters.AddWithValue("@Name", company.Name)
                query.Parameters.AddWithValue("@Cnpj", company.Cnpj)

                companyId = Convert.ToInt32(query.ExecuteScalar())

                MsgBox("Insert realizado com sucesso!", MsgBoxStyle.Information, "Message")

                connection.Close()
            End Using

            If company.Associates.Count > 0 Then
                AddCompanyRelation(companyId, company.Associates)
            End If

        Catch ex As Exception
            MsgBox("Ocorreu um erro durante a inserção: " & ex.Message, MsgBoxStyle.Exclamation, "Erro")
        End Try

    End Sub
    Private Sub AddAssociateRelation(companies As List(Of Company), associateId As Integer)
        Dim query As String = "INSERT INTO T_REL (A_ID, C_ID) VALUES (@a_id, @c_id);"

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            For Each company In companies
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@a_id", associateId)
                    command.Parameters.AddWithValue("@c_id", company.Id)
                    command.ExecuteNonQuery()
                End Using
            Next
        End Using
    End Sub
    Private Sub AddCompanyRelation(companyId As Integer, associates As List(Of Associate))
        Dim query As String = "INSERT INTO T_REL (A_ID, C_ID) VALUES (@a_id, @c_id);"

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            For Each associate In associates
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@a_id", associate.Id)
                    command.Parameters.AddWithValue("@c_id", companyId)
                    command.ExecuteNonQuery()
                End Using
            Next
        End Using
    End Sub
    Public Sub UpdateAssociate(ByVal associate As Associate)
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim query As New SqlCommand("UPDATE T_ASSOC SET A_Name = @Name, A_CPF = @Cpf, A_BIRTH = @BirthDate WHERE ID = @Id", connection)
                query.Parameters.AddWithValue("@Id", associate.Id)
                query.Parameters.AddWithValue("@Name", associate.Name)
                query.Parameters.AddWithValue("@Cpf", associate.Cpf)
                query.Parameters.AddWithValue("@BirthDate", associate.BirthDate)

                query.ExecuteScalar()

                MsgBox("Update realizado com sucesso", MsgBoxStyle.Information, "Message")

                connection.Close()
            End Using

            UpdateAssociateRelation(associate.Companies, associate.Id)

        Catch ex As Exception
            MsgBox("Ocorreu um erro durante a atualização: " & ex.Message, MsgBoxStyle.Exclamation, "Erro")
        End Try
    End Sub
    Public Sub UpdateCompany(ByVal company As Company)
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Dim query As New SqlCommand("UPDATE T_COMP SET C_Name = @Name, C_CNPJ = @Cnpj WHERE ID = @Id", connection)
                query.Parameters.AddWithValue("@Id", company.Id)
                query.Parameters.AddWithValue("@Name", company.Name)
                query.Parameters.AddWithValue("@Cnpj", company.Cnpj)

                query.ExecuteNonQuery()

                MsgBox("Update realizado com sucesso", MsgBoxStyle.Information, "Message")

                connection.Close()
            End Using

            UpdateCompanyRelation(company.Id, company.Associates)

        Catch ex As Exception
            MsgBox("Ocorreu um erro durante a atualização: " & ex.Message, MsgBoxStyle.Exclamation, "Erro")
        End Try

    End Sub
    Private Sub UpdateAssociateRelation(companies As List(Of Company), associateId As Integer)
        Dim deleteQuery As String = "DELETE FROM T_REL WHERE A_ID = @a_id;"

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Using deleteCommand As New SqlCommand(deleteQuery, connection)
                deleteCommand.Parameters.AddWithValue("@a_id", associateId)
                deleteCommand.ExecuteNonQuery()
            End Using

            Dim insertQuery As String = "INSERT INTO T_REL (A_ID, C_ID) VALUES (@a_id, @c_id);"

            For Each company In companies
                Using insertCommand As New SqlCommand(insertQuery, connection)
                    insertCommand.Parameters.AddWithValue("@a_id", associateId)
                    insertCommand.Parameters.AddWithValue("@c_id", company.Id)
                    insertCommand.ExecuteNonQuery()
                End Using
            Next
            connection.Close()
        End Using
    End Sub
    Private Sub UpdateCompanyRelation(companyId As Integer, associates As List(Of Associate))
        Dim deleteQuery As String = "DELETE FROM T_REL WHERE C_ID = @c_id;"

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Using deleteCommand As New SqlCommand(deleteQuery, connection)
                deleteCommand.Parameters.AddWithValue("@c_id", companyId)
                deleteCommand.ExecuteNonQuery()
            End Using

            Dim insertQuery As String = "INSERT INTO T_REL (A_ID, C_ID) VALUES (@a_id, @c_id);"

            For Each associate In associates
                Using insertCommand As New SqlCommand(insertQuery, connection)
                    insertCommand.Parameters.AddWithValue("@a_id", associate.Id)
                    insertCommand.Parameters.AddWithValue("@c_id", companyId)
                    insertCommand.ExecuteNonQuery()
                End Using
            Next
            connection.Close()
        End Using
    End Sub

    Public Sub DeleteAssociate(ByVal associateId As Integer)
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Using deleteRelQuery As New SqlCommand("delete T_REL where A_ID = @Id", connection)
                    deleteRelQuery.Parameters.AddWithValue("@Id", associateId)
                    deleteRelQuery.ExecuteNonQuery()
                End Using
                Using deleteAssocQuery As New SqlCommand("delete T_ASSOC where ID = @Id", connection)
                    deleteAssocQuery.Parameters.AddWithValue("@Id", associateId)
                    deleteAssocQuery.ExecuteNonQuery()
                End Using
                connection.Close()
            End Using

        Catch ex As Exception
            MsgBox("Ocorreu um erro durante o delete: " & ex.Message, MsgBoxStyle.Exclamation, "Erro")
        End Try
    End Sub
    Public Sub DeleteCompany(ByVal companyId As Integer)
        Try
            Using connection As New SqlConnection(connectionString)
                connection.Open()

                Using deleteRelQuery As New SqlCommand("delete T_REL where C_ID = @Id", connection)
                    deleteRelQuery.Parameters.AddWithValue("@Id", companyId)
                    deleteRelQuery.ExecuteNonQuery()
                End Using
                Using deleteAssocQuery As New SqlCommand("delete T_COMP where ID = @Id", connection)
                    deleteAssocQuery.Parameters.AddWithValue("@Id", companyId)
                    deleteAssocQuery.ExecuteNonQuery()
                End Using
                connection.Close()
            End Using
        Catch ex As Exception
            MsgBox("Ocorreu um erro durante o delete: " & ex.Message, MsgBoxStyle.Exclamation, "Erro")
        End Try
    End Sub
End Class
