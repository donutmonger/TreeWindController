import React from 'react'
import {$Meter, $Panel, useDataUpdate} from 'hookui-framework'

const $SettingsPage = ({react, data}) => {
    return <div>
        The wind strength is set to {data}!
    </div>
}

const $TreeWindController = ({react}) => {
    const [data, setData] = react.useState(0)
    useDataUpdate(react, "tree-wind-controller.wind_strength", setData)

    const style = {
        height: "auto"
    }

    return <div>
        <$Panel title="Tree Wind Controller" react={react} style={style}>
            <$SettingsPage react={react} data={data}/>
        </$Panel>
    </div>
}

window._$hookui.registerPanel({
    id: "tree-wind-controller",
    name: "Tree Wind Controller",
    // TODO find a better icon
    icon: "Media/Game/Icons/Trees.svg",
    component: $TreeWindController
})