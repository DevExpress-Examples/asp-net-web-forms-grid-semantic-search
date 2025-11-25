<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ASPxGridViewAIIntegration.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .spin-edit-container {
            display: flex !important;
            align-items: center !important;
        }

        .me-2 {
            margin-right: .5rem !important;
        }
    </style>

    <script type="text/javascript">
        function onSearchChanged(s, e) {
            const text = searchBox.GetText();
            const sim = similaritySpin.GetValue();
            callbackPanel.PerformCallback(JSON.stringify({ search: text, similarity: sim }));
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallbackPanel ID="callbackPanel" runat="server" ClientInstanceName="callbackPanel" OnCallback="CallbackPanel_Callback">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <dx:ASPxGridView ID="ASPxGridView1" runat="server" KeyFieldName="ID" Width="1000" AutoGenerateColumns="False">
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="ID" Width="50px" />
                                <dx:GridViewDataTextColumn FieldName="Name" />
                                <dx:GridViewDataTextColumn FieldName="Description" />
                            </Columns>

                            <Toolbars>
                                <dx:GridViewToolbar>
                                    <Items>
                                        <dx:GridViewToolbarItem Name="similarityFactorSpinEdit" Alignment="Left">
                                            <Template>
                                                <div class="spin-edit-container me-2">
                                                    <span class="me-2">Similarity Factor:</span>
                                                    <dx:ASPxSpinEdit ID="SimilaritySpin" runat="server" ClientInstanceName="similaritySpin" NumberType="Float" Increment="0.05" MinValue="0" MaxValue="1" Width="120px" Number="0.31">
                                                        <ClientSideEvents ValueChanged="onSearchChanged" />
                                                    </dx:ASPxSpinEdit>
                                                </div>
                                            </Template>
                                        </dx:GridViewToolbarItem>
                                        <dx:GridViewToolbarItem Name="searchTextBox" Alignment="Right">
                                            <Template>
                                                <dx:ASPxTextBox ID="SearchBox" runat="server" ClientInstanceName="searchBox" AutoPostBack="false" Width="350px" NullText="Search...">
                                                    <ClientSideEvents TextChanged="onSearchChanged" />
                                                </dx:ASPxTextBox>
                                            </Template>
                                        </dx:GridViewToolbarItem>
                                    </Items>
                                </dx:GridViewToolbar>
                            </Toolbars>
                        </dx:ASPxGridView>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </form>
</body>
</html>
