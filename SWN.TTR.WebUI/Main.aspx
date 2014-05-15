<%@ Page Language="C#" MasterPageFile="~/SWNMaster.Master" AutoEventWireup="true"
    CodeBehind="Main.aspx.cs" Inherits="SWN.TTR.WebUI.Main" Title="[TTR] Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMenu" runat="server">
    <asp:Menu ID="menuMain" runat="server" DataSourceID="SiteMapDataSource1" Orientation="Horizontal"
        SkinID="SWNMenu">
    </asp:Menu>
    <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="false"
        SiteMapProvider="SWNMasterSiteMapProvider" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContents" runat="server">
    <table cellspacing="2">
        <tr>
            <td style="padding-right: 6px;">
                View By:
            </td>
            <td style="padding-right: 6px;">
                <asp:HyperLink ID="hlnkViewMyRequests" runat="server" CssClass="button" Text="My Requests"
                    ToolTip="View My Submitted Requests" NavigateUrl="~/Main.aspx?v=my"></asp:HyperLink>
            </td>
            <td style="padding-right: 6px;">
                <asp:HyperLink ID="hlnkViewPending" runat="server" CssClass="button" Text="My Pending Requests"
                    ToolTip="View My Pending Requests" NavigateUrl="~/Main.aspx?v=mypen"></asp:HyperLink>
            </td>
            <td style="padding-right: 6px;">
                <asp:HyperLink ID="hlnkViewAll" runat="server" CssClass="button" Text="All Requests"
                    ToolTip="View All Requests" NavigateUrl="~/Main.aspx?v=all" OnLoad="CheckViewAllVisibility"></asp:HyperLink>
            </td>
            <td>
            <asp:HyperLink ID="hlnkViewApproved" runat="server" CssClass="button" Text="My Approved Requests"
                    ToolTip="View Requests I have Approved" NavigateUrl="~/Main.aspx?v=myapp"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <asp:MultiView ID="vwRequests" runat="server">
        <asp:View ID="vwMyRequests" runat="server">
            <div class="center">
                <h2>
                    My Requests</h2>
                <div>
                    <asp:GridView ID="gvMyRequests" runat="server" AllowPaging="True" PageSize="20" OnPageIndexChanging="gvMyRequests_PageIndexChanging"
                        AutoGenerateColumns="False" SkinID="SWNGridView" EmptyDataText="(No Records Exist)"
                        EnableViewState="false" Width="100%" AllowSorting="true" OnSorting="gvMyRequests_Sorting"
                        OnRowDataBound="gvMyRequests_RowDataBound" HorizontalAlign="Justify" EditRowStyle-HorizontalAlign="Left">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="20">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkEdit" runat="server" ImageUrl="http://swn-images/Edit.gif"
                                        ToolTip="View Request">
                                    </asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle Width="20px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Event_Name" HeaderText="Event Name" SortExpression="Event_Name"  >
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Event_Start_Date" HeaderText="Event Start" SortExpression="Event_Start_Date"
                                HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="75" >
                                <ItemStyle Width="75px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Requested_DateTime" HeaderText="Requested On" SortExpression="Requested_DateTime"
                                HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="85" >
                                <ItemStyle Width="85px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Requested_By_Full_Name" HeaderText="Requested By" ReadOnly="True"
                                SortExpression="Requested_By_Full_Name" ItemStyle-Width="110" >
                                <ItemStyle Width="110px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status_Description" HeaderText="Status" SortExpression="Status"
                                ItemStyle-Width="110" >
                                <ItemStyle Width="110px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Pending_Review_By_Full_Name" HeaderText="Pending Review By"
                                ReadOnly="True" SortExpression="Pending_Review_By_Full_Name" 
                                ItemStyle-Width="125" >
                                <ItemStyle Width="125px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Total_Cost" HeaderText="Total" SortExpression="Total_Cost"
                                DataFormatString="{0:$0}" ItemStyle-Width="55" >
                                <ItemStyle Width="55px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="vwPendingRequests" runat="server">
            <div class="center">
                <h2>
                    My Pending Requests</h2>
                <div>
                    <asp:GridView ID="gvMyPendingRequests" runat="server" AllowPaging="True" PageSize="20"
                        OnPageIndexChanging="gvAllRequests_PageIndexChanging" AutoGenerateColumns="False"
                        SkinID="SWNGridView" EmptyDataText="(No Records Exist)" EnableViewState="false"
                        Width="100%" AllowSorting="true" OnSorting="gvMyPendingRequests_Sorting" OnRowDataBound="gvMyPendingRequests_RowDataBound">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="20">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkEdit" runat="server" ImageUrl="http://swn-images/Edit.gif"
                                        ToolTip="View Request">
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Event_Name" HeaderText="Event Name" SortExpression="Event_Name" >
                             <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Event_Start_Date" HeaderText="Event Start" SortExpression="Event_Start_Date"
                                HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="75" />
                            <asp:BoundField DataField="Requested_DateTime" HeaderText="Requested On" SortExpression="Requested_DateTime"
                                HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="85" />
                            <asp:BoundField DataField="Requested_By_Full_Name" HeaderText="Requested By" ReadOnly="True"
                                SortExpression="Requested_By_Full_Name" ItemStyle-Width="110" />
                            <asp:BoundField DataField="Status_Description" HeaderText="Status" SortExpression="Status"
                                ItemStyle-Width="110" />
                            <asp:BoundField DataField="Pending_Review_By_Full_Name" HeaderText="Pending Review By"
                                ReadOnly="True" SortExpression="Pending_Review_By_Full_Name" ItemStyle-Width="125" />
                            <asp:BoundField DataField="Total_Cost" HeaderText="Total" SortExpression="Total_Cost"
                                DataFormatString="{0:$0}" ItemStyle-Width="55" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
        <asp:View ID="vwAllRequests" runat="server">
            <div class="center">
                <h2>
                    All Requests</h2>
                <div>
                    <asp:GridView ID="gvAllRequests" runat="server" AllowPaging="True" PageSize="20"
                        OnPageIndexChanging="gvAllRequests_PageIndexChanging" AutoGenerateColumns="False"
                        SkinID="SWNGridView" EmptyDataText="(No Records Exist)" EnableViewState="false"
                        Width="100%" AllowSorting="true" OnSorting="gvAllRequests_Sorting" OnRowDataBound="gvAllRequests_RowDataBound">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="20">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkEdit" runat="server" ImageUrl="http://swn-images/Edit.gif"
                                        ToolTip="View Request">
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Event_Name" HeaderText="Event Name" SortExpression="Event_Name" >
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Event_Start_Date" HeaderText="Event Start" SortExpression="Event_Start_Date"
                                HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="75" />
                            <asp:BoundField DataField="Requested_DateTime" HeaderText="Requested On" SortExpression="Requested_DateTime"
                                HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="85" />
                            <asp:BoundField DataField="Requested_By_Full_Name" HeaderText="Requested By" ReadOnly="True"
                                SortExpression="Requested_By_Full_Name" ItemStyle-Width="110" />
                            <asp:BoundField DataField="Status_Description" HeaderText="Status" SortExpression="Status"
                                ItemStyle-Width="110" />
                            <asp:BoundField DataField="Pending_Review_By_Full_Name" HeaderText="Pending Review By"
                                ReadOnly="True" SortExpression="Pending_Review_By_Full_Name" ItemStyle-Width="125" />
                            <asp:BoundField DataField="Total_Cost" HeaderText="Total" SortExpression="Total_Cost"
                                DataFormatString="{0:$0}" ItemStyle-Width="55" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
      <asp:View ID="vwApprovedRequests" runat="server">
            <div class="center">
                <h2>
                    Approved Requests</h2>
                <div>
                    <asp:GridView ID="gvApprovedRequests" runat="server" AllowPaging="True" PageSize="30"
                        OnPageIndexChanging="gvApprovedRequests_PageIndexChanging" AutoGenerateColumns="False"
                        SkinID="SWNGridView" EmptyDataText="(No Records Exist)" EnableViewState="false"
                        Width="100%" AllowSorting="true" OnSorting="gvApprovedRequests_Sorting" 
                        onrowdatabound="gvApprovedRequests_RowDataBound" >
                        <Columns>
                           <asp:TemplateField ItemStyle-Width="20">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkEdit" runat="server" ImageUrl="http://swn-images/Edit.gif"
                                        ToolTip="View Request">
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Event_Name" HeaderText="Event Name" SortExpression="Event_Name" >
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Event_Start_Date" HeaderText="Event Start" SortExpression="Event_Start_Date"
                                HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="75" />
                            <asp:BoundField DataField="Requested_DateTime" HeaderText="Requested On" SortExpression="Requested_DateTime"
                                HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="85" />
                            <asp:BoundField DataField="Requested_By_Full_Name" HeaderText="Requested By" ReadOnly="True"
                                SortExpression="Requested_By_Full_Name" ItemStyle-Width="110" />
                            <asp:BoundField DataField="Status_Description" HeaderText="Status" SortExpression="Status"
                                ItemStyle-Width="110" />
                            <asp:BoundField DataField="Pending_Review_By" HeaderText="Pending Approval By"
                                ReadOnly="True" SortExpression="Pending_Review_By" ItemStyle-Width="125" />
                                
                            <asp:BoundField DataField="Total_Cost" HeaderText="Total" SortExpression="Total_Cost"
                                DataFormatString="{0:$0}" ItemStyle-Width="55" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
