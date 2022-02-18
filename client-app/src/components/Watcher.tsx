import { Console } from "console";
import * as React from "react";
// import "../styles/SVGWatcher.css";
import { TransformWrapper, TransformComponent } from "react-zoom-pan-pinch";
import Loader from './Loader';

interface Props
{
    isLoading: boolean;
    image: string;
}
// function componentDidMount() {
//     var self = this;
// }

export default function Watcher({image, isLoading}: Props)
{
        return  isLoading ? (<Loader />) : 
        (
            <div className="watcher-block">
                <div id="svgPanel">
                    <div className="content">
                    </div>
                    <TransformWrapper>
                    <TransformComponent>
                    <img style={{overflowY: 'scroll'}} width="750" height="500" src={process.env.PUBLIC_URL+image} alt="svg"></img>
                    </TransformComponent>
                    </TransformWrapper>  
                </div>
            </div>
        )
}
