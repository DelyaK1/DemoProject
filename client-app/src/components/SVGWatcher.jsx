import * as React from "react";
import "../styles/SVGWatcher.css";
import { TransformWrapper, TransformComponent } from "react-zoom-pan-pinch";


function componentDidMount() {
    var self = this;
}

class SVGWatcher extends React.Component {
    render() {
        return (
            <div className="watcher-block">
                <div id="svgPanel">
                    <div className="content">
                        {/* {this.Data} */}
                    </div>
                    <TransformWrapper>
                    <TransformComponent>
                    <img style={{overflowY: 'scroll'}} width="750" height="400" src={process.env.PUBLIC_URL + 'AGCC.287-8441-GA-01-KJ3.DW-0002_А_RU_page_1_page_1.bmp'} alt="svg"></img>
                    </TransformComponent>
                    </TransformWrapper>                   
                    {/* <Loader/> */}
                </div>
            </div>
        )
    }
}

export default SVGWatcher;