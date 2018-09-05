import React from "react";
import { render } from 'react-dom';
import { EidSession } from './components';

import "./css/site.css";

import "bootstrap/dist/js/bootstrap.js";

class Main extends React.Component {
    render() {
        return (<div className="container body">
            <nav className="navbar">
                <div className="container">
                    <label className="navbar-brand">EID application</label>
                </div>
            </nav>
            <EidSession />
        </div>);
    }
}

render(<Main />, document.getElementById('react-app'));