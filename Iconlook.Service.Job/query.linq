<Query Kind="Statements">
  <Reference Relative="..\Iconlook.Client\bin\Debug\netstandard2.1\Iconlook.Client.dll">C:\GitHub\iconlook.server\Iconlook.Client\bin\Debug\netstandard2.1\Iconlook.Client.dll</Reference>
  <Namespace>Iconlook.Client</Namespace>
  <Namespace>Iconlook.Client.Data</Namespace>
  <Namespace>Lykke.Icon.Sdk</Namespace>
  <Namespace>Lykke.Icon.Sdk.Crypto</Namespace>
  <Namespace>Lykke.Icon.Sdk.Data</Namespace>
  <Namespace>Lykke.Icon.Sdk.Transport.Http</Namespace>
  <Namespace>Lykke.Icon.Sdk.Transport.JsonRpc</Namespace>
</Query>

var client = new Iconlook.Client.IconServiceClient();
var response = await client.GetPReps();
var delegated = response[8].GetValidatedBlocks().ToDecimal() / response[8].GetTotalBlocks().ToDecimal();
delegated.Dump();