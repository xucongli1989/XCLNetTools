<%@ Page Title="" Language="C#" MasterPageFile="~/Web.Master" AutoEventWireup="true" CodeBehind="errorHandle.aspx.cs" Inherits="TestWeb.XCLNetTools_Exp.error.errorHandle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <legend>正常AJAX获取</legend>
        <a href="javascript:GetMsg();">提交</a>
    </fieldset>

    <fieldset>
        <legend>AJAX获取异常</legend>
        日期：
        <input type="text" id="txtDate" />（未填，将会出现异常）
        <a href="javascript:GetErrorMsg();">提交</a>
    </fieldset>

    <fieldset>
        <legend>捕捉application error</legend>
        <asp:Button ID="Btn_Error" runat="server" OnClick="Btn_ErrorClick" Text="默认异常输出" />
        <asp:Button ID="Btn_ErrorCustom" runat="server" OnClick="Btn_ErrorCustomClick" Text="自定义异常输出（修改全局的信息写入方法）" />
    </fieldset>

    <script type="text/javascript">
        function GetMsg() {
            $.getJSON(location.href, { "method": "ajax", "type": "add" }, function (data) {
                var errObj = data["<%=XCLNetTools.Message.Log.JsonMessageName%>"];
            if (errObj != null) {
                alert(errObj.Message);
                return;
            }
            alert(data.Message);
        });
        }

        function GetErrorMsg() {
            $.getJSON(location.href, { "method": "ajax", "type": "update", "date": $("#txtDate").val() }, function (data) {
                var errObj = data["<%=XCLNetTools.Message.Log.JsonMessageName%>"];
            if (errObj != null) {
                alert(errObj.Message);
                return;
            }
            alert(data.Message);
        });
        }
    </script>
</asp:Content>