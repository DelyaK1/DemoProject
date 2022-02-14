import { Console } from "console";
import * as React from "react";
// import "../styles/SVGWatcher.css";
import { TransformWrapper, TransformComponent } from "react-zoom-pan-pinch";

interface Props
{
    image: string;
}
// function componentDidMount() {
//     var self = this;
// }

export default function Watcher({image}: Props)
{
    console.log(image);
        return (
            <div className="watcher-block">
                <div id="svgPanel">
                    <div className="content">
                    </div>
                    <TransformWrapper>
                    <TransformComponent>
                    <img style={{overflowY: 'scroll'}} width="750" height="400" src={process.env.PUBLIC_URL+image} alt="svg"></img>
                    </TransformComponent>
                    </TransformWrapper>  
                </div>
            </div>
        )
}
