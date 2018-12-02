# Get started

https://safe-stack.github.io/docs/quickstart/#create-your-first-safe-app

1) Install fake as global tool (``dotnet tool install fake-cli -g``
2) ``fake build --target run``

This should restore everything, build it and eventuall open a browser.

# What I wanted to do

Use Office calendar (pretty neat) with Fable and get a datetime back in my Elmish model.


# What did I do?

I cloned https://github.com/0x53A/FableTsBabylonJsSample/ and
removed babylonjs and added [office-ui-fabric-react](https://developer.microsoft.com/en-us/fabric#/components)

I removed previous tsx files, and added files from [office react documentation](https://developer.microsoft.com/en-us/fabric#/components)

I followed react documentation https://reactjs.org/docs/lifting-state-up.html
I added a function to React Props of my componenet.
This function was taking a DateTime and returning void/unit

It added this signature both on F# and TS
```f#
type CalendarProps =
    | GetState of (DateTime -> unit)
```

```js
export interface ICalendarProps {
  // new function from Client.fsx
  getState : ((date : Date ) => void)
```

## F# additional modification in Client.fs
I used this function
```f#
    (DateTime -> unit)
```
in the update Elmish part
```f#
Calendar [ GetState  (fun d -> (GetDate d) |> dispatch )  ]
```

## TypeScript additionnal modifcation

At the very end of CalendarComponent.tsx
```tsx
    // using my function injected from props
    this.props.getState(date)
```

