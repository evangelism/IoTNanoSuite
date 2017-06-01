<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebPortal._Default" %>
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Sensor Data</h1>

    <asp:DropDownList runat="server" AutoPostBack="True" ID="ddDispType">
        <asp:ListItem Value="X,Y" />
        <asp:ListItem Value="Temp" />
        <asp:ListItem Value="Wind" />
        <asp:ListItem Value="Power" />
    </asp:DropDownList>

    <asp:Chart ID="MainChart" runat="server" Height="600" Width="800">
      <series>
         <asp:Series Name="MainSeries" ChartArea="MainChartArea" ChartType="Line" Color="Black"/>
         <asp:Series Name="RealSeries" ChartArea="MainChartArea" ChartType="Line" Color="Red" />
      </series>
      <chartareas>
         <asp:ChartArea Name="MainChartArea" />
      </chartareas>
    </asp:Chart>

</asp:Content>
