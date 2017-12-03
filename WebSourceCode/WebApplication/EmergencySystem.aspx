<%@ Page Title="Request Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmergencySystem.aspx.cs" Inherits="WebApplication.EmergencySystem" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Request Emergency Vehicle Here!</h1>
    </div>
    
        <asp:GridView ID="GridView1" runat="server" ShowFooter="True" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowCommand="GridView1_RowCommand" style="margin-left:auto; margin-right:auto;"  >
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Request Id" SortExpression="RequestId"  ControlStyle-Width="100px" >
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" style="margin-left: 0px; margin-right:100px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("requestId") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lbInsert" CommandName="Insert" runat="server" ForeColor="White" OnClick="Insert">Process Request</asp:LinkButton>
                    </FooterTemplate>

                <ControlStyle Width="100px"></ControlStyle>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="Vehicle Type" SortExpression="VehicleType"  ControlStyle-Width="100px">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" style="margin-left: 0px; margin-right:80px">
                            <asp:ListItem>Select Vehicle Type</asp:ListItem>
                            <asp:ListItem>Ambulance</asp:ListItem>
                            <asp:ListItem>FireTruck</asp:ListItem>
                            <asp:ListItem>PoliceCar</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("vehicleType") %>'></asp:Label>
                    </ItemTemplate>

                    <FooterTemplate>
                        <asp:DropDownList ID="ddlVehicleType" runat="server" style="margin-left: 0px; margin-right:80px">
                            <asp:ListItem>Select vehicle type</asp:ListItem>
                            <asp:ListItem>Ambulance</asp:ListItem>
                            <asp:ListItem>FireTruck</asp:ListItem>
                            <asp:ListItem>PoliceCar</asp:ListItem>
                        </asp:DropDownList>
                        
                        <asp:RequiredFieldValidator ID="rfvVehicleType" runat=server ControlToValidate="ddlVehicleType" Text="*" ErrorMessage="Vehicle Type required." 
                            ForeColor="Red" InitialValue="Select vehicle type"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Zip Code" SortExpression="ZipCode"  ControlStyle-Width="100px">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" style="margin-left: 0px; margin-right:60px" Text='<%# Bind("zipCode") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("zipCode") %>'></asp:Label>
                    </ItemTemplate>
                    
                    <FooterTemplate>
                        <asp:TextBox ID="txtZipCode" runat="server" style="margin-left: 0px; margin-right:60px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvZipCode" runat=server ControlToValidate="txtZipCode" Text="*" ErrorMessage="Zip code required." 
                            ForeColor="Red"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vehicle ID" SortExpression="VehicleId"  ControlStyle-Width="100px">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" style="margin-left: 0px; margin-right:60px" Text='<%# Bind("vehicleId") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("vehicleId") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblVehicleID" runat="server" style="margin-left: 0px; margin-right:60px" Text="TBD"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Gap" SortExpression="Gap"  ControlStyle-Width="100px">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox5" runat="server" style="margin-left: 0px; margin-right:60px" Text='<%# Bind("gap") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("gap") %>'></asp:Label>
                    </ItemTemplate>

                    <FooterTemplate>
                        <asp:Label ID="lblGap" runat="server" style="margin-left: 0px; margin-right:60px" Text="TBD"></asp:Label>
                    </FooterTemplate>

                </asp:TemplateField>

               <%-- <asp:TemplateField>
                    
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" CommandArgument='<%#Eval("requestId") %>' CommandName="Delete" runat="server" style="margin-left: 0px; margin-right:130px">Delete</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>

            </Columns>
            <EditRowStyle BackColor="#999999" />
            
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
  
    <br />
    <div style="margin-left:auto; margin-right:auto; align-content:center">
    <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red" runat="server" />
    </div>
    <asp:Button runat="server" Text="Delete"  CausesValidation="false" />
</asp:Content>
