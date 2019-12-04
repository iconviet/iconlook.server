<Query Kind="Statements">
  <Reference Relative="..\Iconlook.Client\bin\Debug\netstandard2.1\Iconlook.Client.dll">C:\GitHub\iconlook.server\Iconlook.Client\bin\Debug\netstandard2.1\Iconlook.Client.dll</Reference>
  <Namespace>Iconlook.Client</Namespace>
  <Namespace>Iconlook.Client.Service</Namespace>
  <Namespace>Iconlook.Client.Tracker</Namespace>
  <Namespace>Lykke.Icon.Sdk</Namespace>
  <Namespace>Lykke.Icon.Sdk.Crypto</Namespace>
  <Namespace>Lykke.Icon.Sdk.Data</Namespace>
  <Namespace>Lykke.Icon.Sdk.Transport.Http</Namespace>
  <Namespace>Lykke.Icon.Sdk.Transport.JsonRpc</Namespace>
</Query>

var client = new Iconlook.Client.Tracker.IconTrackerClient();
var response = await client.GetPReps();
response.Data.Dump();