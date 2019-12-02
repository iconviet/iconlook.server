<Query Kind="Statements">
  <Reference Relative="bin\Debug\netcoreapp3.1\Iconlook.Service.Job.dll">C:\GitHub\iconlook.server\Iconlook.Service.Job\bin\Debug\netcoreapp3.1\Iconlook.Service.Job.dll</Reference>
  <NuGetReference>Lykke.Icon.Sdk</NuGetReference>
  <Namespace>Iconlook.Service.Job</Namespace>
  <Namespace>Iconlook.Service.Job.Blockchain</Namespace>
  <Namespace>Iconlook.Service.Job.PRep</Namespace>
  <Namespace>Lykke.Icon.Sdk</Namespace>
  <Namespace>Lykke.Icon.Sdk.Crypto</Namespace>
  <Namespace>Lykke.Icon.Sdk.Data</Namespace>
  <Namespace>Lykke.Icon.Sdk.Transport.Http</Namespace>
  <Namespace>Lykke.Icon.Sdk.Transport.JsonRpc</Namespace>
</Query>

var client = new Iconlook.Service.Job.IconClient();
var call = new Call.Builder()
			.Method("getPReps")
			.To(new Address("cx0000000000000000000000000000000000000000"))
			.From(new Address("hx0000000000000000000000000000000000000000"))
			.Build();
var item = await client.CallAsync(call);
item.ToObject().GetItem("preps").ToArray().Dump();