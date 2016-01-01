<%@ Page Title="" Language="C#" MasterPageFile="~/Web.Master" AutoEventWireup="true" CodeBehind="OutExcel.aspx.cs" Inherits="TestWeb.XCLNetTools_Exp.DataHandler.OutExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="btnClick" OnClick="btn_Click" runat="server" Text="导出" />
</asp:Content>