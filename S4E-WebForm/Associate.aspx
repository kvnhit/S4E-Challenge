<%@ Page Title="About" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Associate.aspx.vb" Inherits="S4E_WebForm.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
		<div>
			<h1>Cadastro de Associados</h1>
		</div>
		<hr />
		<div>
			<asp:Label ID="lblId" runat="server" Font-Bold="True" Font-Size="Medium" Text="Id"></asp:Label>
            <br />
            <asp:TextBox ID="txtId" runat="server" Width="200px" MaxLength="200"></asp:TextBox>
            <br />
            <br />
		    <asp:Label ID="lblName" runat="server" Font-Bold="True" Font-Size="Medium" Text="Nome"></asp:Label>
            <br />
            <asp:TextBox ID="txtName" runat="server" Width="200px" MaxLength="200"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lblCpf" runat="server" Font-Bold="True" Font-Size="Medium" Text="CPF"></asp:Label>
            <br />
            <asp:TextBox ID="txtCpf" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lblBirth" runat="server" Font-Bold="True" Font-Size="Medium" Text="Data de Nascimento"></asp:Label>
            <br />
            <asp:TextBox ID="txtBirth" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
            <br />
            <br />
            <fieldset style="border: 1px solid #000; display: inline-block; padding: 10px;">
                <legend>Empresas</legend>
                <div style="display: flex; flex-direction: row; align-items: center" >
                    <div style="display: flex; flex-direction: column;">
                        <asp:Label ID="lblAvailableCompanies" runat="server" Font-Bold="True" Font-Size="Medium" Text="Empresas disponíveis"></asp:Label>
                        <asp:ListBox ID="ListBox1" runat="server" Width="200px"></asp:ListBox>
                    </div>
                    <div style="display: flex; flex-direction: column; margin: 0 10px;">
                        <asp:Button ID="btnAdd" runat="server" Text="&gt;&gt;" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnRemove" runat="server" Text="&lt;&lt;" OnClick="btnRemove_Click" />
                    </div>
                    <div style="display: flex; flex-direction: column;">
                        <asp:Label ID="lblSelectedCompanies" runat="server" Font-Bold="True" Font-Size="Medium" Text="Empresas selecionadas"></asp:Label>
                        <asp:ListBox ID="ListBox2" runat="server" Width="200px"></asp:ListBox>
                        <br />
                    </div>
                </div>
            </fieldset>
            <hr />
            <div align="center">
                <div style="text-align: left;">
                    <asp:Label ID="lblFilter" runat="server" Text="Filtrar por ID: " Font-Bold="True" Font-Size="Medium"></asp:Label>

                    <asp:TextBox ID="txtIdFilter" runat="server" Width="100px"></asp:TextBox>

                    <asp:Button ID="btnIdFilter" runat="server" Text="Pesquisar" Width="85px" />

                </div>
                <br />
                <asp:GridView ID="gridViewAssoc" runat="server" Width="80%">
                    <AlternatingRowStyle Font-Size="Medium" />
                    <Columns>
                        <asp:ButtonField ButtonType="Button" CommandName="Select" HeaderText="Selecionar Associado" ShowHeader="True" Text="Selecionar" />
                    </Columns>
                </asp:GridView>
            </div>
            <hr />
            <asp:Button ID="btnInsert" runat="server" Font-Bold="True" Text="Inserir" Width="200px" />
            <asp:Button ID="btnUpdate" runat="server" Font-Bold="True" Text="Atualizar" Width="200px" />
            <asp:Button ID="btnDelete" runat="server" Font-Bold="True" Text="Deletar" Width="200px" OnClientClick="return confirm('Tem certeza que deseja deletar?')" />
            <br />

		</div>
    </main>
</asp:Content>
