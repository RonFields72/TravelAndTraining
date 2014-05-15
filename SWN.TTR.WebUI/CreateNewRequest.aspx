<%@ Page Title="[TTR] Create New Request" Language="C#" MasterPageFile="~/SWNMaster.Master"
    AutoEventWireup="True" CodeBehind="CreateNewRequest.aspx.cs" Inherits="SWN.TTR.WebUI.CreateNewRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <script type="text/javascript" language="javascript" src="scripts/jscripts.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMenu" runat="server">
    <table class="menu">
        <tbody>
            <tr>
                <td>
                    <asp:HyperLink ID="lnkbtnHome" runat="server" Text="Home" CssClass="menu" onmouseover="this.className='menumouseover';"
                        onmouseout="this.className='menu';" NavigateUrl="~/Main.aspx" ToolTip="Home"></asp:HyperLink>
                </td>
                <td>
                    <asp:LinkButton ID="lnkbtnSave" runat="server" Text="Save" CssClass="menu" onmouseover="this.className='menumouseover';"
                        onmouseout="this.className='menu';" OnClick="SaveRequest" ToolTip="Save">
                    </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lnkbtnDelete" runat="server" Text="Delete" CssClass="menu" onmouseover="this.className='menumouseover';"
                        onmouseout="this.className='menu';" OnClick="DeleteRequest" ToolTip="Delete Request"
                        OnClientClick="return confirmDeleteRequest();">
                    </asp:LinkButton>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContents" runat="server">

    <script type="text/javascript">

        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }

        // Add click handlers for buttons to show and hide modal popup on pageLoad
        function pageLoad() {
            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);
        }

        function ChangeEventEndDate(e) {
            var ceStart = $find("ceEventStartDate");
            var start = ceStart.get_selectedDate();
            if (start == null)
                return;
            var ceEnd = $find("ceEventEndDate");
            var end = ceEnd.get_selectedDate();
            if (end == null) {
                ceEnd.set_selectedDate(start);
            }
            else {
                if (start > end)
                    ceEnd.set_selectedDate(start);
            }
        }
        
        

        function ChangeTrainEndDate(e) {
            var ceStart = $find("ceTrainStartDate");
            var start = ceStart.get_selectedDate();
            if (start == null)
                return;
            var ceEnd = $find("ceTrainEndDate");
            var end = ceEnd.get_selectedDate();
            if (end == null) {
                ceEnd.set_selectedDate(start);
            }
            else {
                if (start > end)
                    ceEnd.set_selectedDate(start);
            }
        }
    </script>

    <asp:ScriptManagerProxy ID="proxy" runat="server">
        <Services>
            <asp:ServiceReference Path="~/services/EmployeeService.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <p class="status">
        <asp:Literal ID="litStatus" runat="server" EnableViewState="false"></asp:Literal></p>
    <asp:Wizard ID="wzCreateRequest" runat="server" SkinID="SWNWizard" SideBarStyle-Width="21%"
        Width="100%" OnActiveStepChanged="wzCreateRequest_ActiveStepChanged" OnInit="wzCreateRequest_Init"
        OnPreviousButtonClick="wzCreateRequest_PreviousButtonClick" OnNextButtonClick="wzCreateRequest_NextButtonClick"
        ActiveStepIndex="0">
        <SideBarStyle Width="21%"></SideBarStyle>
        <FinishNavigationTemplate>
            <i></i>
        </FinishNavigationTemplate>
        <WizardSteps>
            <asp:WizardStep ID="wzEventInfo" runat="server" Title="Enter Event Information">
                <h2>
                    Enter Your Event Information</h2>
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <th align="left">
                            Event:
                        </th>
                        <td colspan="3" style="color: #FF0000">
                            <asp:TextBox ID="txtEvent" runat="server" Rows="2" Columns="80" TextMode="MultiLine">                                
                            </asp:TextBox>
                            *<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEvent"
                                ErrorMessage="Required" Font-Size="Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Event Start Date:
                        </th>
                        <td style="color: #FF0000">
                            <asp:TextBox ID="txtEventStartDate" runat="server" Width="75"></asp:TextBox>
                            <asp:ImageButton ID="ibtnEventStart" runat="server" AlternateText="Choose a Training Start Date"
                                ImageUrl="http://swn-images/calendar.png" CausesValidation="False" />
                            <cc1:CalendarExtender ID="ceEventStartDate" runat="server" TargetControlID="txtEventStartDate"
                                Format="MM/dd/yyyy" PopupButtonID="ibtnEventStart" OnClientDateSelectionChanged="ChangeEventEndDate"
                                BehaviorID="ceEventStartDate">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="meEventStart" runat="server" Mask="99/99/9999" MaskType="Date"
                                TargetControlID="txtEventStartDate">
                            </cc1:MaskedEditExtender>
                            *<cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="meEventStart"
                                ControlToValidate="txtEventStartDate" Display="Dynamic" EmptyValueMessage="<%$ Resources:Resources, DateRequired %>"
                                ErrorMessage="*" MinimumValueBlurredMessage="*" InvalidValueMessage="<%$ Resources:Resources, DateInvalid %>"
                                IsValidEmpty="False" MaximumValue="<%$ Resources:Resources, DateMaximumValue %>"
                                MaximumValueBlurredMessage="*" MaximumValueMessage="<%$ Resources:Resources, DateMaximumValueExceeded %>"
                                ToolTip="Please enter a start date.">
                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             </cc1:MaskedEditValidator>
                        </td>
                        <th>
                            Event End Date:
                        </th>
                        <td style="color: #FF0000">
                            <asp:TextBox ID="txtEventEndDate" runat="server" Width="75"></asp:TextBox>
                            <asp:ImageButton ID="ibtnEventEnd" runat="server" AlternateText="Choose a Training Start Date"
                                ImageUrl="http://swn-images/calendar.png" CausesValidation="False" />
                            <cc1:CalendarExtender ID="ceEventEndDate" runat="server" TargetControlID="txtEventEndDate"
                                Format="MM/dd/yyyy" PopupButtonID="ibtnEventEnd" BehaviorID="ceEventEndDate">
                            </cc1:CalendarExtender>
                            <cc1:MaskedEditExtender ID="meEventEnd" runat="server" Mask="99/99/9999" MaskType="Date"
                                TargetControlID="txtEventEndDate">
                            </cc1:MaskedEditExtender>
                            *<cc1:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="meEventStart"
                                ControlToValidate="txtEventEndDate" Display="Dynamic" EmptyValueMessage="<%$ Resources:Resources, DateRequired %>"
                                ErrorMessage="*" InvalidValueMessage="<%$ Resources:Resources, DateInvalid %>"
                                IsValidEmpty="False" MaximumValue="<%$ Resources:Resources, DateMaximumValue %>"
                                MaximumValueBlurredMessage="*" MaximumValueMessage="<%$ Resources:Resources, DateMaximumValueExceeded %>"
                                ToolTip="Please enter an end date.">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </cc1:MaskedEditValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            City:
                        </th>
                        <td style="color: #FF0000">
                            <asp:TextBox ID="txtEventCity" runat="server" MaxLength="50" Width="150">                                
                            </asp:TextBox>
                            *<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEventCity"
                                ErrorMessage="Required" Font-Bold="False" Font-Size="Small"></asp:RequiredFieldValidator>
                        </td>
                        <th>
                            State:
                        </th>
                        <td style="color: #FF0000">
                            <asp:DropDownList ID="ddlEventStates" runat="server" DataTextField="Name" DataValueField="Id"
                                OnInit="LoadEventStates" OnDataBound="AddDefaultEventState">
                            </asp:DropDownList>
                            *<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlEventStates"
                                ErrorMessage="Required" InitialValue="-" Font-Size="Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Business Purpose of Event:
                        </th>
                        <td colspan="3" style="color: #FF0000">
                            <asp:TextBox ID="txtEventPurpose" runat="server" MaxLength="100" TextMode="MultiLine"
                                Rows="2" Columns="75">                                
                            </asp:TextBox>
                            *<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEventPurpose"
                                ErrorMessage="Required" Font-Size="Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="2">
                            If event is specifically associated with a project,<br />
                            list the project name.
                        </th>
                        <td colspan="2">
                            <asp:TextBox ID="txtEventProjects" runat="server" MaxLength="50" Width="300"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="4">
                            <asp:CheckBox ID="ckbTraining" runat="server" Text="Request Training?" OnLoad="LoadTrainingPanel" />
                            <asp:CheckBox ID="ckbTravel" runat="server" Text="Request Travel?" OnLoad="LoadTravelPanel" />
                        </th>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep ID="wzTravelTraining" runat="server" Title="Enter Travel/Training Information">
                <h2>
                    Enter Travel and Training Information</h2>
                <asp:Panel ID="pnlTraining" runat="server" GroupingText="<b>Training Information</b>">
                    <table cellpadding="3" cellspacing="3">
                        <tr>
                            <th>
                                Class/Seminar/Conference:
                            </th>
                            <td colspan="2" style="color: #FF0000">
                                <asp:TextBox ID="txtTrainName" runat="server" MaxLength="100" Width="307px" CssClass="txt"
                                    Height="21px"></asp:TextBox>
                                *<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtTrainName"
                                    ErrorMessage="Required" Font-Size="Small"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Start Date:
                            </th>
                            <td style="color: #FF0000" class="style3">
                                <asp:TextBox ID="txtTrainStart" runat="server" Width="75"></asp:TextBox>
                                <asp:ImageButton ID="ibtnTrainStart" runat="server" AlternateText="Choose a Training Start Date"
                                    ImageUrl="http://swn-images/calendar.png" />
                                <cc1:CalendarExtender ID="ceTrainStart" runat="server" TargetControlID="txtTrainStart"
                                    Format="MM/dd/yyyy" PopupButtonID="ibtnTrainStart" BehaviorID="ceTrainStartDate"
                                    OnClientDateSelectionChanged="ChangeTrainEndDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meTrainStart" runat="server" Mask="99/99/9999" MaskType="Date"
                                    TargetControlID="txtTrainStart">
                                </cc1:MaskedEditExtender>
                                *<cc1:MaskedEditValidator ID="MaskedEditValidator4" runat="server" ControlExtender="meEventStart"
                                    ControlToValidate="txtTrainStart" Display="Dynamic" EmptyValueMessage="<%$ Resources:Resources, DateRequired %>"
                                    ErrorMessage="Required" Font-Size="Small" InvalidValueMessage="<%$ Resources:Resources, DateInvalid %>"
                                    IsValidEmpty="False" MaximumValue="<%$ Resources:Resources, DateMaximumValue %>"
                                    MaximumValueBlurredMessage="*" MaximumValueMessage="<%$ Resources:Resources, DateMaximumValueExceeded %>"
                                    ToolTip="Please enter an end date.">

                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                </cc1:MaskedEditValidator>
                            </td>
                            <th class="style1">
                                End Date:
                            </th>
                            <td style="color: #FF0000">
                                <asp:TextBox ID="txtTrainEnd" runat="server" Width="75"></asp:TextBox>
                                <asp:ImageButton ID="ibtnTrainEnd" runat="server" AlternateText="Choose a Training End Date"
                                    ImageUrl="http://swn-images/calendar.png" />
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTrainEnd"
                                    Format="MM/dd/yyyy" PopupButtonID="ibtnTrainEnd" BehaviorID="ceTrainEndDate">
                                </cc1:CalendarExtender>
                                <cc1:MaskedEditExtender ID="meTrainEnd" runat="server" Mask="99/99/9999" MaskType="Date"
                                    TargetControlID="txtTrainEnd">
                                </cc1:MaskedEditExtender>
                                *<cc1:MaskedEditValidator ID="MaskedEditValidator3" runat="server" ControlExtender="meEventStart"
                                    ControlToValidate="txtTrainEnd" Display="Dynamic" EmptyValueMessage="<%$ Resources:Resources, DateRequired %>"
                                    ErrorMessage="Required" Font-Size="Small" InvalidValueMessage="<%$ Resources:Resources, DateInvalid %>"
                                    IsValidEmpty="False" MaximumValue="<%$ Resources:Resources, DateMaximumValue %>"
                                    MaximumValueBlurredMessage="*" MaximumValueMessage="<%$ Resources:Resources, DateMaximumValueExceeded %>"
                                    ToolTip="Please enter an end date.">
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 </cc1:MaskedEditValidator>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Estimated Cost:
                            </th>
                            <td rowspan="0" style="color: #000000" class="style3" colspan="0">
                                <asp:TextBox ID="txtTrainCost" Style="text-align: right" runat="server" MaxLength="5"
                                    Width="50" onkeypress="return isNumberKey(event);" OnInit="txtTrainCost_Init">
                                </asp:TextBox>
                                .00<font color="Red">*</font><asp:RequiredFieldValidator ID="RequiredFieldValidator6"
                                    runat="server" ErrorMessage="Required" ControlToValidate="txtTrainCost" Font-Size="Small"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtTrainCost"
                                    MinimumValue="1" ErrorMessage="Cost must be greater than 0." 
                                    Type="Currency" MaximumValue="100000000" Font-Size="Small"></asp:RangeValidator>
                            </td>
                            <th align="left" colspan="0" rowspan="0" style="color: #FF0000" class="style1">
                                &nbsp;
                            </th>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlTravel" runat="server" GroupingText="<b>Travel Information</b>">
                    <table cellpadding="3" cellspacing="3">
                        <tr>
                            <th>
                                Destination City:
                            </th>
                            <td style="color: #FF0000">
                                <asp:TextBox ID="txtTravelCity" runat="server" MaxLength="50" Width="150">                                
                                </asp:TextBox>
                                *<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtTravelCity"
                                    ErrorMessage="Required" Font-Size="Small"></asp:RequiredFieldValidator>
                            </td>
                            <th>
                                Destination State:
                            </th>
                            <td style="color: #FF0000">
                                <asp:DropDownList ID="ddlTravelStates" runat="server" DataTextField="Name" DataValueField="Id"
                                    OnInit="LoadTravelStates" OnDataBound="AddDefaultTravelState">
                                </asp:DropDownList>
                                *<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddlTravelStates"
                                    ErrorMessage="Required" InitialValue="-" Font-Size="Small"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <b>
                                    <asp:CheckBox ID="ckbTravelAir" runat="server" Text="Traveling by Air?" AutoPostBack="true"
                                        OnCheckedChanged="AirTransportationChanged" OnLoad="LoadAirPanel" />
                                    <asp:CheckBox ID="ckbTravelLodging" runat="server" Text="Need Lodging?" AutoPostBack="true"
                                        OnCheckedChanged="LodgingChanged" OnLoad="LoadLodgingPanel" />
                                    <asp:CheckBox ID="ckbTravelGround" runat="server" Text="Need Ground Transportation?"
                                        AutoPostBack="true" OnCheckedChanged="GroundTransportationChanged" OnLoad="LoadGroundPanel" />
                                </b>
                            </td>
                        </tr>
                    </table>
                    <div style="margin: 8px 5px 0px 5px;">
                        <asp:Panel ID="pnlAir" runat="server" GroupingText="<b>Air</b>">
                            <asp:SqlDataSource ID="sqldsAirMode" runat="server" ConnectionString="<%$ ConnectionStrings:SWN.TTR.Properties.Settings.cnTTR %>"
                                SelectCommand="swn_sp_Air_Transportation_Retrieve" SelectCommandType="StoredProcedure">
                            </asp:SqlDataSource>
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <td colspan="4">
                                        <b>
                                            <asp:CheckBox ID="ckbAirInteroffice" runat="server" Text="Shuttle?" OnLoad="LoadInterOffice"
                                                AutoPostBack="true" OnCheckedChanged="InterOfficeChanged" /></b>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Destination Mode:
                                    </th>
                                    <td style="color: #FF0000">
                                        <asp:DropDownList ID="ddlAirDestinationMode" runat="server" DataTextField="Description"
                                            DataValueField="Id" OnInit="LoadDestinationAirModes" OnDataBound="AddDefaultDestinationAirMode">
                                        </asp:DropDownList>
                                        *<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="ddlAirDestinationMode"
                                            ErrorMessage="Required" InitialValue="NONE" Font-Size="Small"></asp:RequiredFieldValidator>
                                    </td>
                                    <th>
                                        Return Mode:
                                    </th>
                                    <td style="color: #FF0000">
                                        <asp:DropDownList ID="ddlAirReturnMode" runat="server" DataTextField="Description"
                                            DataValueField="Id" OnInit="LoadReturnAirModes" OnDataBound="AddDefaultReturnAirMode">
                                        </asp:DropDownList>
                                        *<asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="ddlAirReturnMode"
                                            ErrorMessage="Required" InitialValue="NONE" Font-Size="Small"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Estimated Cost:
                                    </th>
                                    <td style="color: #000000">
                                        <asp:TextBox ID="txtAirCost" Style="text-align: right" runat="server" MaxLength="5"
                                            Width="40" onkeypress="return isNumberKey(event);">
                                        </asp:TextBox>.00<font color="Red">*</font><asp:RequiredFieldValidator ID="RequiredFieldValidator8"
                                            runat="server" ControlToValidate="txtAirCost" ErrorMessage="Required" Font-Size="Small"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtAirCost"
                                            MinimumValue="1" ErrorMessage="Cost must be greater than 0." 
                                            Type="Currency"  MaximumValue="10000000" Font-Size="Small"></asp:RangeValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="margin: 8px 5px 0px 5px;">
                        <asp:Panel ID="pnlLodging" runat="server" GroupingText="<b>Lodging</b>">
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <th>
                                        # Of Nights:
                                    </th>
                                    <td style="color: #FF0000">
                                        <asp:TextBox ID="txtLodgingNights" runat="server" MaxLength="2" Width="25" onkeypress="return isNumberKey(event);">
                                        </asp:TextBox>
                                        *<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtLodgingNights"
                                            ErrorMessage="Required" Font-Size="Small"></asp:RequiredFieldValidator>
                                    </td>
                                    <th>
                                        Estimated Cost:
                                    </th>
                                    <td style="color: #000000">
                                        <asp:TextBox ID="txtLodgingCost" runat="server" Style="text-align: right" MaxLength="5"
                                            Width="40" onkeypress="return isNumberKey(event);">
                                        </asp:TextBox>.00<font color="Red">*</font><asp:RequiredFieldValidator ID="RequiredFieldValidator9"
                                            runat="server" ControlToValidate="txtLodgingCost" ErrorMessage="Required" Font-Size="Small"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtLodgingCost"
                                            MinimumValue="1" Type="Currency" 
                                            ErrorMessage="Cost must be greater than 0." MaximumValue="10000000" 
                                            Font-Size="Small"></asp:RangeValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="margin: 8px 5px 5px 5px;">
                        <asp:Panel ID="pnlGround" runat="server" GroupingText="<b>Ground</b>">
                            <asp:SqlDataSource ID="sqldsGroundModes" runat="server" ConnectionString="<%$ ConnectionStrings:SWN.TTR.Properties.Settings.cnTTR %>"
                                SelectCommand="swn_sp_Ground_Transportation_Retrieve" SelectCommandType="StoredProcedure">
                            </asp:SqlDataSource>
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <th>
                                        Ground Mode(s):
                                    </th>
                                    <td>
                                        <asp:CheckBoxList ID="ckblstGroundModes" runat="server" DataTextField="Description"
                                            DataValueField="Id" OnInit="LoadGroundModes" OnDataBound="RemoveDefaultGroundMode"
                                            RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Estimated Cost:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtGroundCost" runat="server" Style="text-align: right" MaxLength="5"
                                            Width="40" onkeypress="return isNumberKey(event);">
                                        </asp:TextBox>.00<font color="Red">*</font><asp:RequiredFieldValidator ID="RequiredFieldValidator11"
                                            runat="server" ControlToValidate="txtGroundCost" ErrorMessage="Required" Font-Size="Small"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtGroundCost"
                                            MinimumValue="1" Type="Currency" 
                                            ErrorMessage="Cost must be greater than 0." MaximumValue="10000000" 
                                            Font-Size="Small"></asp:RangeValidator>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </asp:Panel>
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <th>
                            General Comments: meals, parking,<br />
                            cab fare if no rental, etc.:
                        </th>
                        <td style="color: #FF0000">
                            <asp:TextBox ID="txtMiscComments" runat="server" TextMode="MultiLine" Rows="3" Columns="75">
                            </asp:TextBox>
                            *<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtMiscComments"
                                ErrorMessage="Required" Font-Size="Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Estimated Cost (General Comments):
                        </th>
                        <td style="color: #000000">
                            <asp:TextBox ID="txtMiscCost" runat="server" Style="text-align: right" MaxLength="5"
                                Width="40" onkeypress="return isNumberKey(event);"></asp:TextBox>
                            .00<font color="Red">*</font><asp:RequiredFieldValidator ID="RequiredFieldValidator12"
                                runat="server" ControlToValidate="txtMiscCost" ErrorMessage="Required" Font-Size="Small"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtMiscCost"
                                MinimumValue="1" Type="Currency" 
                                ErrorMessage="Cost must be greater than 0." MaximumValue="10000000" 
                                Font-Size="Small"></asp:RangeValidator>
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep ID="wzConfirmation" runat="server" Title="Confirm" EnableViewState="false">
                <h2>
                    Confirm Your Request</h2>
                <asp:Panel ID="pnlcnfEvent" runat="server" GroupingText="<b>Event Details</b>" EnableViewState="false">
                    <table cellpadding="3" cellspacing="3">
                        <tr>
                            <th align="justify">
                                Event Name:
                            </th>
                            <td>
                                <asp:Literal ID="litcEventName" runat="server"></asp:Literal>
                            </td>
                            <th>
                            </th>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Event Start Date:
                            </th>
                            <td>
                                <asp:Literal ID="litcEventStart" runat="server"></asp:Literal>
                            </td>
                            <th>
                                Event End Date:
                            </th>
                            <td>
                                <asp:Literal ID="litcEventEnd" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Event Location:
                            </th>
                            <td>
                                <asp:Literal ID="litcEventCity" runat="server"></asp:Literal>,
                                <asp:Literal ID="litcEventState" runat="server"></asp:Literal>
                            </td>
                            <th>
                            </th>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Business Purpose of Event:
                            </th>
                            <td colspan="3">
                                <asp:Literal ID="litcEventPurpose" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2">
                                If event is specifically associated with a project,<br />
                                list the project name:
                            </th>
                            <td colspan="2">
                                <asp:Literal ID="litcEventProjects" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlcnfTraining" runat="server" GroupingText="<b>Training Details</b>"
                    EnableViewState="false">
                    <table cellpadding="3" cellspacing="3">
                        <tr>
                            <th>
                                Class/Seminar/Conference:
                            </th>
                            <td colspan="3">
                                <asp:Literal ID="litcTrainName" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Start Date:
                            </th>
                            <td>
                                <asp:Literal ID="litcTrainStart" runat="server"></asp:Literal>
                            </td>
                            <th>
                                End Date:
                            </th>
                            <td>
                                <asp:Literal ID="litcTrainEnd" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Estimated Cost
                            </th>
                            <td>
                                $<asp:Literal ID="litcTrainCost" runat="server"></asp:Literal>.00
                            </td>
                            <th>
                            </th>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlcnfTravel" runat="server" GroupingText="<b>Travel Details</b>"
                    EnableViewState="false">
                    <table cellpadding="5" cellspacing="5">
                        <tr>
                            <th>
                                Destination:
                            </th>
                            <td colspan="3">
                                <asp:Literal ID="litcTravelCity" runat="server"></asp:Literal>,
                                <asp:Literal ID="litcTravelState" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                    <div style="margin: 8px 5px 0px 5px;">
                        <asp:Panel ID="pnlcnfAir" runat="server" GroupingText="<b>Air</b>" EnableViewState="false">
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <th>
                                        Destination Mode:
                                    </th>
                                    <td>
                                        <asp:Literal ID="litcAirDest" runat="server"></asp:Literal>
                                    </td>
                                    <th>
                                        Return Mode:
                                    </th>
                                    <td>
                                        <asp:Literal ID="litcAirReturn" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        Estimated Cost:
                                    </th>
                                    <td>
                                        $<asp:Literal ID="litcAirCost" runat="server"></asp:Literal>.00
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="margin: 8px 5px 0px 5px;">
                        <asp:Panel ID="pnlcnfLodging" runat="server" GroupingText="<b>Lodging</b>" EnableViewState="false">
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <th>
                                        # Of Nights:
                                    </th>
                                    <td>
                                        <asp:Literal ID="litcLodgingNights" runat="server"></asp:Literal>
                                    </td>
                                    <th>
                                        Estimated Cost:
                                    </th>
                                    <td>
                                        $<asp:Literal ID="litcLodgingCost" runat="server"></asp:Literal>.00
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="margin: 8px 5px 0px 5px;">
                        <asp:Panel ID="pnlcnfGround" runat="server" GroupingText="<b>Ground</b>" EnableViewState="false">
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <th>
                                        Ground Mode(s):
                                    </th>
                                    <td>
                                        <asp:Literal ID="litcGroundModes" runat="server"></asp:Literal>
                                    </td>
                                    <th>
                                        Estimated Cost:
                                    </th>
                                    <td>
                                        $<asp:Literal ID="litcGroundCost" runat="server"></asp:Literal>.00
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </asp:Panel>
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <th>
                            General Comments:
                        </th>
                        <td>
                            <asp:Literal ID="litcGenComments" runat="server" EnableViewState="false"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Estimated Cost (General Comments):
                        </th>
                        <td>
                            $<asp:Literal ID="litcMiscCost" runat="server" EnableViewState="false"></asp:Literal>.00
                        </td>
                    </tr>
                    <tr>
                        <th>
                        </th>
                        <td>
                        </td>
                    </tr>
                </table>
                <hr />
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <th>
                            Total Estimated Cost of Request:
                        </th>
                        <th>
                            $<asp:Literal ID="litcTotalCost" runat="server" EnableViewState="false"></asp:Literal>.00
                        </th>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <span class="status">If you are satisified with your request, click Next to submit your
                                request.</span>
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep ID="wzSubmit" runat="server" Title="Submit Request">
                <h2>
                    Submit Your Request</h2>
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <th>
                            Reviewer/Approver:
                        </th>
                        <td style="color: #FF0000">
                            <asp:TextBox ID="txtReviewers" runat="server" AutoCompleteType="None" CssClass="unwatermarked">
                            </asp:TextBox>
                            <cc1:AutoCompleteExtender ID="autoCompleteEx" runat="server" ServicePath="~/services/EmployeeService.asmx"
                                ServiceMethod="GetCompletionList" EnableCaching="true" CompletionInterval="500"
                                CompletionSetCount="20" MinimumPrefixLength="2" TargetControlID="txtReviewers">
                            </cc1:AutoCompleteExtender>
                            <cc1:TextBoxWatermarkExtender ID="waterMarkReviewers" runat="server" TargetControlID="txtReviewers"
                                WatermarkText="Type In Reviewer" WatermarkCssClass="watermarked">
                            </cc1:TextBoxWatermarkExtender>
                            *<asp:RequiredFieldValidator ID="reqReviewer" runat="server" ControlToValidate="txtReviewers"
                                EnableClientScript="true" Text="Required" Display="Static" ErrorMessage="Required"
                                ValidationGroup="Submit">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Button ID="btnSubmitRequest" runat="server" Text="Submit Request" SkinID="DefaultButton"
                                ToolTip="Submit Request" OnClick="SubmitRequest" CausesValidation="true" ValidationGroup="Submit" />
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none" />
    <cc1:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
        BackgroundCssClass="modalBackground" DropShadow="True" PopupDragHandleControlID="programmaticPopupDragHandle"
        RepositionMode="RepositionOnWindowScroll">
    </cc1:ModalPopupExtender>
    <asp:Panel runat="server" CssClass="modalPopup" ID="programmaticPopup" Style="display: none;
        width: 500px; padding: 10px">
        <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black; text-align: center;">
            <b>Request Submission Errors</b></asp:Panel>
        
            &nbsp;<asp:BulletedList ID="blstErrors" runat="server" BulletStyle="Circle" ForeColor="Red"
                DisplayMode="Text" EnableViewState="false">
            </asp:BulletedList>
            <p class="center">
                <input id="hideModalPopupViaClientButton" type="button" value="OK" />
            </p>
            
         
       
    </asp:Panel>
</asp:Content>
