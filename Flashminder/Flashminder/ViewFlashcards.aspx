<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewFlashcards.aspx.cs" Inherits="Flashminder.ViewFlashcards" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div>
            <asp:Label ID="category_lbl" runat="server">Category: </asp:Label>
            <asp:DropDownList ID="category_dropdownlist" runat="server" DataTextField="CategoryName" DataValueField="Id" AppendDataBoundItems="true" OnSelectedIndexChanged="SwitchCategory" AutoPostBack="true" >
                <asp:ListItem  Selected="True" Text="All" Value="All"></asp:ListItem>
            </asp:DropDownList>
        </div>
	    <asp:DataList ID="flashcards_datalist" runat="server" >
		    <HeaderTemplate>      
                <table>      
                    <tr>      
                        <td>      
                            <h1>Flashcards</h1>      
                        </td>      
                    </tr>      
                </table>      
            </HeaderTemplate>    
            <ItemTemplate>
                <div>
                    Front:
                    <div>
                        <asp:Image runat="server" Height="300" Width="500" ID="FrontImage" 
                            Visible='<%# Eval("FrontImage") == null ? false : System.IO.File.Exists(Server.MapPath(("Images/"+ Eval("FrontImage").ToString()))) %>'
                            ImageUrl='<%# ("Images/"+ Eval("FrontImage")?.ToString())%>'/>
                        <br />
                        <%# Eval("FrontText")%>
                    </div>
                </div>
                <div>
                    Back:
                    <div>
                        <asp:Image runat="server" Height="300" Width="500" ID="BackImage" 
                            Visible='<%# Eval("BackImage") == null ? false : System.IO.File.Exists(Server.MapPath(("Images/"+ Eval("BackImage").ToString()))) %>'
                            ImageUrl='<%# ("Images/"+ Eval("BackImage")?.ToString())%>'/>
                        <br />
                        <%# Eval("BackText")%>
                    </div>
                </div>
            </ItemTemplate>
	    </asp:DataList>
    </div>
</asp:Content>
