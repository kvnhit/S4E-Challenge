<%@ Page Title="Contact" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Company.aspx.vb" Inherits="S4E_WebForm.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
		<div>
			<h1>Cadastro de Empresas</h1>
		</div>
		<hr />
		<div>
		    <asp:Label ID="lblName" runat="server" Font-Bold="True" Font-Size="Medium" Text="Nome"></asp:Label>
            <br />
            <asp:TextBox ID="txtName" runat="server" Width="200px" MaxLength="200"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lblCnpj" runat="server" Font-Bold="True" Font-Size="Medium" Text="CNPJ"></asp:Label>
            <br />
            <asp:TextBox ID="txtCnpj" runat="server" Width="200px" MaxLength="14"></asp:TextBox>
            <br />
            <br />
            <fieldset style="border: 1px solid #000; display: inline-block; padding: 10px;">
                <legend>Associados</legend>
                <div style="display: flex; flex-direction: row; align-items: center" >
                    <div style="display: flex; flex-direction: column;">
                        <asp:Label ID="lblAvailableAssociates" runat="server" Font-Bold="True" Font-Size="Medium" Text="Associados disponíveis"></asp:Label>
                        <asp:ListBox ID="ListBox1" runat="server" Width="200px"></asp:ListBox>
                    </div>
                    <div style="display: flex; flex-direction: column; margin: 0 10px;">
                        <asp:Button ID="btnAdd" runat="server" Text="&gt;&gt;" Font-Bold="True" Font-Size="Medium" Width="50px" />
                        <asp:Button ID="btnRemove" runat="server" Text="&lt;&lt;" Font-Bold="True" Font-Size="Medium" Width="50px" />
                    </div>
                    <div style="display: flex; flex-direction: column;">
                        <asp:Label ID="lblSelectedAssociates" runat="server" Font-Bold="True" Font-Size="Medium" Text="Associados selecionadas"></asp:Label>
                        <asp:ListBox ID="ListBox2" runat="server" Width="200px"></asp:ListBox>
                    </div>
                </div>
            </fieldset>
            <hr />
            <div align="center">
                <div style="text-align: left; display: flex; align-items: center;">
                    <div>
                        <asp:Label ID="lblFilter" runat="server" Text="Filtrar por ID: " Font-Bold="True" Font-Size="Medium"></asp:Label>

                        <asp:TextBox ID="txtIdFilter" runat="server" Width="100px"></asp:TextBox>

                        <asp:Button ID="btnIdFilter" runat="server" Text="Pesquisar" Width="85px" />
                    </div>
                    <div style="padding: 10px; display: flex; align-items: center; margin-right: 10px;">
                        <asp:RadioButtonList ID="RadioButtonFilters" GroupName="filterGroup" runat="server" style="margin-right: 10px;" RepeatDirection="Horizontal">
                                <asp:ListItem style="margin-right: 10px;" Value="name" Selected="True">Nome</asp:ListItem>
                                <asp:ListItem style="margin-right: 10px;" Value="cnpj">CNPJ</asp:ListItem>
                            </asp:RadioButtonList>
                        <div style="margin-right: 10px;">
                            <asp:TextBox ID="txtOtherFilter" runat="server" Width="100px"></asp:TextBox>
                            <asp:Button ID="btnIdFilter1" runat="server" Text="Pesquisar" Width="85px" />
                        </div>
                    </div>
                </div>
                <br />
                <asp:GridView ID="gridViewComp" runat="server" Width="80%">
                    <AlternatingRowStyle Font-Size="Medium" />
                    <Columns>
                        <asp:ButtonField ButtonType="Button" CommandName="Select" HeaderText="Selecionar Empresa" ShowHeader="True" Text="Selecionar" />
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
