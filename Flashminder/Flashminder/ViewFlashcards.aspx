<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewFlashcards.aspx.cs" Inherits="Flashminder.ViewFlashcards" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript" src="Scripts/jquery-2.2.3.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
				 $('.flashcard').click(function (e) {

                    var target = $(e.currentTarget).context;

					if (target.classList.contains('flashcard')) {
                        if (target.children[0].innerText) {
                            window.location.href = 'ViewFlashcard.aspx?FlashcardID=' + target.children[0].innerText;
                        }
                    }
                });
            });

		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">

        <div>
            <asp:Label ID="category_lbl" runat="server">Category: </asp:Label>
            <asp:DropDownList ID="category_dropdownlist" runat="server" DataTextField="CategoryName" DataValueField="Id" AppendDataBoundItems="true" OnSelectedIndexChanged="SwitchCategory" AutoPostBack="true" >
                <asp:ListItem  Selected="True" Text="All" Value="All"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <h1>Flashcards</h1>   
	    <asp:DataList ID="flashcards_datalist" runat="server" >
		    <HeaderTemplate>      
                <table style="width:100%">
                    <tr>
                        <td style="width:50%;margin-left:95px;display:block">Front</td>
                        <td style="width:50%">Back</td>
                    </tr>
                </table>

            </HeaderTemplate>    
            <ItemTemplate >
                    <div class="flashcard" runat="server">
                        <div id="id" class="flashcard-id" hidden="hidden" ><%# Eval("Id") %></div>
                        <table style="width:100%; border-style:solid">
                            <tr>
                                <td style="">
                                    <button class="btn btn-danger" style="width:75px;margin-right:20px;" onclick="return confirm('Are you sure you want to delete this item?');">Delete</button>
                                </td>
                                <td style="width:50%">                    
                                    <div>
                                        <div>
                                            <asp:Image runat="server" Height="300" Width="500" ID="FrontImage" 
                                                Visible='<%# Eval("FrontImage") == null ? false : System.IO.File.Exists(Server.MapPath(("Images/"+ Eval("FrontImage").ToString()))) %>'
                                                ImageUrl='<%# ("Images/"+ Eval("FrontImage")?.ToString())%>'/>
                                            <br />
                                            <%# Eval("FrontText")%>
                                        </div>
                                    </div>
                                </td>
                                <td style="width:50%">
                                    <div>
                                        <div>
                                            <asp:Image runat="server" Height="300" Width="500" ID="BackImage" 
                                                Visible='<%# Eval("BackImage") == null ? false : System.IO.File.Exists(Server.MapPath(("Images/"+ Eval("BackImage").ToString()))) %>'
                                                ImageUrl='<%# ("Images/"+ Eval("BackImage")?.ToString())%>'/>
                                            <br />
                                            <%# Eval("BackText")%>
                                        </div>
                                    </div>
                                </td>
                           </tr>
                        </table>
                    </div>
            </ItemTemplate>
	    </asp:DataList>
    </div>
</asp:Content>
