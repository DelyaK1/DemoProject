import * as React from "react"
import axios from "axios";

import "../styles/SVGWatcher.css"
import Loader from "./Loader";
import * as Data from "../assets/json/XMLData.xml"

function componentDidMount() {
    var self = this;
    axios
        .get("https://fakerestapi.azurewebsites.net/api/Authors", {
            "Content-Type": "application/xml; charset=utf-8"
        })
        .then(function(response) {
            self.setState({ response: response.data });
        })
        .catch(function(error) {
            console.log(error);
        });
}

class SVGWatcher extends React.Component {
    render() {
        return (
            <div className="watcher-block">
                <div className="svgTools">

                    <div className="svgTool-btn">
                        <input className="neu-checkbox" name="ncb" id="svgSwitchColor" type="checkbox" value=""/>
                            <label htmlFor="svgSwitchColor" className="neu btn neu-form me-2">
                                <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor"
                                     className="neu-image-svg" viewBox="0 0 16 16">
                                    <path visibility="visible"
                                          d="M7 8a3.5 3.5 0 0 1 3.5 3.555.5.5 0 0 0 .625.492A1.503 1.503 0 0 1 13 13.5a1.5 1.5 0 0 1-1.5 1.5H3a2 2 0 1 1 .1-3.998.5.5 0 0 0 .509-.375A3.502 3.502 0 0 1 7 8zm4.473 3a4.5 4.5 0 0 0-8.72-.99A3 3 0 0 0 3 16h8.5a2.5 2.5 0 0 0 0-5h-.027z">
                                    </path>
                                    <path visibility="visible" className="neu-checkbox-icon checkbox-c" fill="#5c93c3"
                                          d="M11.286 1.778a.5.5 0 0 0-.565-.755 4.595 4.595 0 0 0-3.18 5.003 5.46 5.46 0 0 1 1.055.209A3.603 3.603 0 0 1 9.83 2.617a4.593 4.593 0 0 0 4.31 5.744 3.576 3.576 0 0 1-2.241.634c.162.317.295.652.394 1a4.59 4.59 0 0 0 3.624-2.04.5.5 0 0 0-.565-.755 3.593 3.593 0 0 1-4.065-5.422z">
                                    </path>
                                    <path visibility="hidden" className="neu-checkbox-icon checkbox-h" fill="#ffda9e"
                                          d="M10.5 1.5a.5.5 0 0 0-1 0v1a.5.5 0 0 0 1 0v-1zm3.743 1.964a.5.5 0 1 0-.707-.707l-.708.707a.5.5 0 0 0 .708.708l.707-.708zm-7.779-.707a.5.5 0 0 0-.707.707l.707.708a.5.5 0 1 0 .708-.708l-.708-.707zm1.734 3.374a2 2 0 1 1 3.296 2.198c.199.281.372.582.516.898a3 3 0 1 0-4.84-3.225c.352.011.696.055 1.028.129zm4.484 4.074c.6.215 1.125.59 1.522 1.072a.5.5 0 0 0 .039-.742l-.707-.707a.5.5 0 0 0-.854.377zM14.5 6.5a.5.5 0 0 0 0 1h1a.5.5 0 0 0 0-1h-1z">
                                    </path>
                                </svg>
                            </label>
                    </div>

                </div>

                <div id="svgPanel">
                    <div className="content">
                        {this.Data}
                    </div>
                    <Loader/>
                </div>
            </div>
        )
    }
}

export default SVGWatcher;