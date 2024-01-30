import React from 'react'
import {$Meter, $Panel, useDataUpdate} from 'hookui-framework'

// TODO Not sure why they need this, useDataUpdate seems to do the trick?
const engineEffect = (react, event, setFunc) => {
    const updateEvent = event + ".update"
    const subscribeEvent = event + ".subscribe"
    const unsubscribeEvent = event + ".unsubscribe"

    return react.useEffect(() => {
        var clear = engine.on(updateEvent, (data) => {
            // console.log(updateEvent, data)
            if (data.current !== undefined && data.min !== undefined && data.max !== undefined) {
                const percentage = ((data.current - data.min) / (data.max - data.min)) * 100;
                setFunc(percentage);
            } else {
                // console.warn(`${updateEvent} didn't have the expected properties`, data);
                setFunc(data);
            }
        })
        engine.trigger(subscribeEvent)
        return () => {
            engine.trigger(unsubscribeEvent)
            clear.clear();
        };
    }, [])
}

const $SettingsPage = ({react, data}) => {
    let keys = Object.keys(data)
    const meters = keys.map((k) => {
        const {label, eventName, gradientName, isEnabled} = data[k]
        if (isEnabled) {
            const [meterState, setMeterState] = react.useState(-1)
            engineEffect(react, eventName, setMeterState)
            // TODO Slider can be found here:
            //   https://github.com/Cities2Modding/LegacyFlavour/blob/main/LegacyFlavour.Frontend/src/jsx/components/_slider.jsx
            return <$Meter key={eventName} label={label} value={meterState} gradient={gradientName}/>
        }
    }).filter(i => i)

    return <div>
        {...meters}
    </div>
}

const $TreeWindController = ({react}) => {
    const [data, setData] = react.useState({})
    useDataUpdate(react, "tree-wind-controller.settings", setData)

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
    icon: "Media/Game/Icons/Trees.svg",
    component: $TreeWindController
})