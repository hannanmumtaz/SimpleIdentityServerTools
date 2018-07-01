import React, { Component } from "react";
import { ViewClients } from './common';

class OpenidClients extends Component {
	render() {
		return ( <ViewClients type="openid" /> );
	}
}

export default OpenidClients;