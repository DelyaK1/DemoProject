import * as React from "react";
import "../styles/Loader.css"

class Loader extends React.Component {
    render() {
        return (
            <div className="loader-wrap">
                <div className="loader"></div>
            </div>)
    }
}

export default Loader;