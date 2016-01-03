<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiChoicePlatform.aspx.cs" Inherits="StudentWebRole.MultiChoicePlatform" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Windows Azure Blob Storage</title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <style type="text/css">
        body { font-family: Verdana; font-size: 12px; }
        h1 { font-size:x-large; font-weight:bold; }
        h2 { font-size:large; font-weight:bold; }
        img { margin:2em;
        }
        li { list-style: none; }
        ul { padding:1em; }
        
        .form { width:50em; }
        .form li span {width:30%; float:left; font-weight:bold; }
        .form li input { width:70%; float:left; }
        .form input { float:right; }
        
        .item { font-size:smaller; font-weight:bold; }
        .item ul li { padding:0.25em; background-color:#ffeecc; }
        .item ul li span { padding:0.25em; background-color:#ffffff; display:block; font-style:italic; font-weight:normal; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Multi Choice</h1>
        <div class="form">
            <ul>
                <li><span>Image</span><asp:FileUpload ID="imageFile" runat="server" /></li>
                <li><span>Correct Answer</span><asp:TextBox ID="correctAnswer" runat="server"/></li>
                <li><span>Incorrect Answer</span><asp:TextBox ID="wrongAnswer1" runat="server"/></li>
                <li><span>IncorrectAnswer</span><asp:TextBox ID="wrongAnswer2" runat="server"/></li>
                <li><span>IncorrectAnswer</span><asp:TextBox ID="wrongAnswer3" runat="server"/></li>
            </ul>
            <asp:Button ID="upload" runat="server" onclick="upload_Click" Text="Publish" />
        </div>
        <div style=" float:left;">
        Status: <asp:Label ID="status" runat="server" />
        </div>
        <asp:ListView ID="images" runat="server" OnItemDeleting="OnListItemDeleting" 
            onitemdatabound="OnBlobDataBound">
            <LayoutTemplate>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </LayoutTemplate>
            <EmptyDataTemplate>
                <h2>No Data Available</h2>
            </EmptyDataTemplate>            
            <ItemTemplate>            
                <div class="item">
                    <ul style="width:40em;float:left;clear:left" >
                    <asp:Repeater ID="blobMetadata" runat="server">
                    <ItemTemplate>
                        <li><%# Eval("Name") %><span><%# Eval("Value") %></span></li>
                    </ItemTemplate>
                    </asp:Repeater>
                        <li>
                     
                        
                        <asp:LinkButton ID="deleteBlob" 
                                OnClientClick="return confirm('Delete image?');"
                                CommandName="Delete" 
                                CommandArgument='<%# Eval("Name")%>'
                                runat="server" Text="Delete" oncommand="OnDeleteImage" />
                         
                        <asp:LinkButton ID="CopyBlob" 
                                OnClientClick="return confirm('Copy image?');"
                                CommandName="Copy" 
                                CommandArgument='<%# Eval("Name")%>'
                                runat="server" Text="Copy" oncommand="OnCopyImage" />

                            <a href='<%# Eval("uri")%>'>THUMB</a>
                                                          
                               
                                  
                        </li>
                    </ul>                    
                    <img src="<%# Eval("Uri") %>" alt="<%# Eval("Uri") %>" style="float:left"/>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>
    </form>
</body>
</html>