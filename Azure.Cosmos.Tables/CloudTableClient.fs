[<AutoOpen>]
module PolyCoder.Azure.Cosmos.Table.CloudTableClientModule

open Microsoft.Azure.Cosmos.Table
open System
open System.Threading
open FSharp.Control

let tableReference
    (tableName: string)
    (client: CloudTableClient) =
  client.GetTableReference(tableName)

module Cancellable =
  let getServiceProperties
      (cancellationToken: CancellationToken)
      (client: CloudTableClient) =
    client.GetServicePropertiesAsync(cancellationToken)

  let getServicePropertiesWith
      (cancellationToken: CancellationToken)
      (requestOptions: TableRequestOptions)
      (operationContext: OperationContext)
      (client: CloudTableClient) =
    client.GetServicePropertiesAsync(requestOptions, operationContext, cancellationToken)

  let setServiceProperties
      (cancellationToken: CancellationToken)
      (properties: ServiceProperties)
      (client: CloudTableClient) =
    client.SetServicePropertiesAsync(properties, cancellationToken)

  let setServicePropertiesWith
      (cancellationToken: CancellationToken)
      (requestOptions: TableRequestOptions)
      (operationContext: OperationContext)
      (properties: ServiceProperties)
      (client: CloudTableClient) =
    client.SetServicePropertiesAsync(properties, requestOptions, operationContext, cancellationToken)
    
  let getServiceStats
      (cancellationToken: CancellationToken)
      (client: CloudTableClient) =
    client.GetServiceStatsAsync(cancellationToken)

  let getServiceStatsWith
      (cancellationToken: CancellationToken)
      (requestOptions: TableRequestOptions)
      (operationContext: OperationContext)
      (client: CloudTableClient) =
    client.GetServiceStatsAsync(requestOptions, operationContext, cancellationToken)

  let listTablesWith
      (cancellationToken: CancellationToken)
      (requestOptions: TableRequestOptions)
      (operationContext: OperationContext)
      (prefix: string)
      (client: CloudTableClient) =
    Common.Cancellable.segmentedAsAsyncSeq
      cancellationToken
      (fun cToken contToken -> client.ListTablesSegmentedAsync(prefix, Nullable(), contToken, requestOptions, operationContext, cToken) |> Async.AwaitTask)
      (fun segment -> segment.ContinuationToken)
      (fun segment -> seq { yield! segment })
      null

  let listTables
      (cancellationToken: CancellationToken)
      (prefix: string)
      (client: CloudTableClient) =
    Common.Cancellable.segmentedAsAsyncSeq
      cancellationToken
      (fun cToken contToken -> client.ListTablesSegmentedAsync(prefix, Nullable(), contToken, cToken) |> Async.AwaitTask)
      (fun segment -> segment.ContinuationToken)
      (fun segment -> seq { yield! segment })
      null

  let listAllTablesWith
      (cancellationToken: CancellationToken)
      (requestOptions: TableRequestOptions)
      (operationContext: OperationContext)
      (client: CloudTableClient) =
    listTablesWith cancellationToken requestOptions operationContext null client

  let listAllTables
      (cancellationToken: CancellationToken)
      (client: CloudTableClient) =
    listTables cancellationToken null client

let getServiceProperties client =
  client |> Cancellable.getServiceProperties CancellationToken.None 

let getServicePropertiesWith requestOptions operationContext client =
  client |> Cancellable.getServicePropertiesWith CancellationToken.None requestOptions operationContext

let setServiceProperties properties client =
  client |> Cancellable.setServiceProperties CancellationToken.None properties

let setServicePropertiesWith requestOptions operationContext properties client =
  client |> Cancellable.setServicePropertiesWith CancellationToken.None properties requestOptions operationContext

let getServiceStats client =
  client |> Cancellable.getServiceStats CancellationToken.None

let getServiceStatsWith requestOptions operationContext client =
  client |> Cancellable.getServiceStatsWith CancellationToken.None requestOptions operationContext

let listTablesWith requestOptions operationContext prefix client =
  client |> Cancellable.listTablesWith CancellationToken.None requestOptions operationContext prefix

let listTables prefix client =
  client |> Cancellable.listTables CancellationToken.None prefix

let listAllTablesWith requestOptions operationContext client =
  client |> Cancellable.listAllTablesWith CancellationToken.None requestOptions operationContext

let listAllTables client =
  client |> Cancellable.listAllTables CancellationToken.None
