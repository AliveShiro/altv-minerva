import * as alt from 'alt-client'

import './Login'
import './PlayerHud'
import './VehicleHud'
import './VehicleRadio'
import './Notifications'

let view: alt.WebView = null

alt.onServer('UiManager:Initialize', async (url: string) => {
    // this call can take a while, thats why we have async function
    if (view) {
        view.destroy()
    }

    view = new alt.WebView(url)
    view.focus()

    view.on("loaded", () => {
        view.emit('ToggleComponent', 'Login', true)
        alt.emit('UiManager:Loaded')
    })

    view.on('ui:EmitClient', (eventName, ...args) => {
        alt.emit(eventName, args)
    })

    view.on('ui:EmitServer', (eventName, ...args) => {
        alt.emit(eventName, args)
    })
})

const uiCopyToClipboardHandler = (contents: string) => {
    view.emit('CopyToClipboard', contents)
}
alt.on('UiManager:CopyToClipboard', uiCopyToClipboardHandler)
alt.onServer('UiManager:CopyToClipboard', uiCopyToClipboardHandler)

alt.on('UiManager:Emit', (eventName: string, ...args: any[]) => {
    if (view === null) return
    view.emit(eventName, ...args)
})

alt.on('UiManager:ShowComponent', (component: string) => {
    if (view === null) return
    view.emit('ToggleComponent', component, true)
})

alt.on('UiManager:HideComponent', (component: string) => {
    if (view === null) return
    view.emit('ToggleComponent', component, false)
})

alt.on('UiManager:ToggleComponent', (component: string) => {
    if (view === null) return
    view.emit('ToggleComponent', component)
})

alt.on('UiManager:SetAppData', (key: string, value: any) => {
    if (view === null) return
    view.emit('SetAppData', key, value)
})