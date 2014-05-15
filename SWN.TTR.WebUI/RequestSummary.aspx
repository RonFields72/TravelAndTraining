<%@ Page Title="[TTR] Request Summary" Language="C#" MasterPageFile="~/SWNMaster.Master" AutoEventWireup="true" CodeBehind="RequestSummary.aspx.cs" Inherits="SWN.TTR.WebUI.RequestSummary" %>
<%@ Register Src="~/Controls/RequestSummary.ascx" TagName="RequestSummary" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMenu" runat="server">
    <asp:Menu ID="menuMain" runat="server" DataSourceID="SiteMapDataSource1" Orientation="Horizontal" SkinID="SWNMenu">                                             
    </asp:Menu>
    <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" 
        ShowStartingNode="false" 
        SiteMapProvider="SWNMasterSiteMapProvider" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContents" runat="server">
    <uc1:RequestSummary ID="ucSummary" runat="server"></uc1:RequestSummary>
</asp:Content>
