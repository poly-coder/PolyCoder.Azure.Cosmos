[<AutoOpen>]
module PolyCoder.Azure.Cosmos.Table.Common

open System.Threading
open FSharp.Control

module Cancellable =
  let segmentedAsAsyncSeq
      (cancellationToken: CancellationToken)
      (getNextSegment: CancellationToken -> 'contToken -> Async<'segment>)
      (getNextContToken: 'segment -> 'contToken)
      (getSegmentItems: 'segment -> 'item seq)
      (initToken: 'contToken) =
    let rec loop token = asyncSeq {
      cancellationToken.ThrowIfCancellationRequested()

      let! segment = getNextSegment cancellationToken token

      for item in getSegmentItems segment do
        yield item

      let nextToken = getNextContToken segment

      if isNull nextToken |> not then
        yield! loop nextToken
    }

    loop initToken

let segmentedAsAsyncSeq
    (getNextSegment: 'contToken -> Async<'segment>)
    (getNextContToken: 'segment -> 'contToken)
    (getSegmentItems: 'segment -> 'item seq)
    (initToken: 'contToken) =
  Cancellable.segmentedAsAsyncSeq
    CancellationToken.None
    (fun _ nextToken -> getNextSegment nextToken)
    getNextContToken
    getSegmentItems
    initToken
