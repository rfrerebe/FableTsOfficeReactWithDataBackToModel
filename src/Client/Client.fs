module Client

open Elmish
open Elmish.React

open Fable.Core
open Fable.Core.JsInterop

open Fable.PowerPack.Fetch

open Shared

open Fulma
open System

// The model holds data that you want to keep track of while the application is running
// in this case, we are keeping track of a counter
// we mark it as optional, because initially it will not be available from the client
// the initial value will be requested from server
type Model = {  Date: DateTime option }

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
| GetDate of DateTime

// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<Msg> =
    let initialModel = { Date = None }
    initialModel, Cmd.none

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match msg with
    | GetDate d  -> { currentModel with Date = Some d }, Cmd.none

// -------------------------------------------------------
// Import the TypeScript componen

type CalendarProps =
    | GetState of (DateTime -> unit)

open Fable.Helpers.React
open Fable.Helpers.React.Props

let inline Calendar (props : CalendarProps list) : Fable.Import.React.ReactElement =
    ofImport "default" "./CalendarComponent.tsx" (keyValueList CaseRules.LowerFirst props) []

// -------------------------------------------------------


let safeComponents =
    let components =
        span [ ]
           [
             a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
             str ", "
             a [ Href "http://fable.io" ] [ str "Fable" ]
             str ", "
             a [ Href "https://elmish.github.io/elmish/" ] [ str "Elmish" ]
             str ", "
             a [ Href "https://mangelmaxime.github.io/Fulma" ] [ str "Fulma" ]
           ]

    p [ ]
        [ strong [] [ str "SAFE Template" ]
          str " powered by: "
          components ]


let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ]
        [ str txt ]


let view (model : Model) (dispatch : Msg -> unit) =
    div []
        [ Navbar.navbar [ Navbar.Color IsPrimary ]
            [ Navbar.Item.div [ ]
                [ Heading.h2 [ ]
                    [ str "SAFE Template" ] ] ]

          Container.container []
              [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ Heading.h3 [] [ str ("Select a model to view ") ] ]
                Columns.columns [] [
                      Column.column [] [
                        Calendar [ GetState  (fun d -> (GetDate d) |> dispatch )  ]]
                      Column.column [] [
                        (match model.Date with
                        | Some d -> str (sprintf "Date :%A" d)
                        | None -> str "No Date selected")
                    ]]

                // // Render the Typescript Component
                //
              ]

          Footer.footer [ ]
                [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ safeComponents ] ] ]


#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReactUnoptimized "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
