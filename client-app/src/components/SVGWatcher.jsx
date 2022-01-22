import * as React from "react"
import axios from "axios";
import Image from '../assets/img/AGCC.287-1751-TL.PID-2000-01_A_RU_PAGE1.png'

import "../styles/SVGWatcher.css"
import Loader from "./Loader";
import * as Data from "../assets/json/XMLData.xml"
import agent from "../app/api/agent";

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
                    <img style={{overflowY: 'scroll'}} width="750" height="400" src={Image} alt="svg"></img>

                    {/* <Loader/> */}
                </div>
            </div>
        )
    }
}

export default SVGWatcher;