[<AutoOpen>]
module PolyCoder.Azure.Cosmos.Table.CloudStorageAccountModule

open Microsoft.Azure.Cosmos.Table

let parseStorageAccount (connectionString: string) =
  CloudStorageAccount.Parse connectionString

let tryParseStorageAccount (connectionString: string) =
  CloudStorageAccount.TryParse connectionString
    |> function
      | true, account -> Some account
      | false, _ -> None

let developmentStorageAccount =
  CloudStorageAccount.DevelopmentStorageAccount

let tableClient (account: CloudStorageAccount) =
  account.CreateCloudTableClient()

let tableClientWith options (account: CloudStorageAccount) =
  account.CreateCloudTableClient(options)
