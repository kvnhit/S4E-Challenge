<%@ Page Title="Seja Bem Vindo" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="S4E_WebForm._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %></h2>

        <p>
            Escolha abaixo o cadastro que deseja seguir.
        </p>

        <div>
            <div>
                <asp:HyperLink ID="lnk1" runat="server" NavigateUrl="Associate.aspx">Cadastrar Associado</asp:HyperLink>
                <br />
            </div>
            <div>
                <asp:HyperLink ID="lnk2" runat="server" NavigateUrl="Company.aspx">Cadastrar Empresa</asp:HyperLink>
            </div>
        </div>

        <br />
        <address>
            <strong>Support:</strong><a href="mailto:Support@example.com">kevinhitzschky@gmail.com</a><br />
        </address>
    </main>
</asp:Content>
