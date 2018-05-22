import React, { Component } from 'react';
import { NativeModules, StatusBar, View } from 'react-native';
import { COLOR, ThemeProvider } from 'react-native-material-ui';
import { AppRegistry, Text } from 'react-native';

import Container from '../Container';
import MainTabNavigator from '../routes';

const UIManager = NativeModules.UIManager;
const uiTheme = {
    palette: {
        primaryColor: COLOR.green500,
        accentColor: COLOR.pink500,
    },
};

class App extends Component {
    componentWillMount() {
        if (UIManager.setLayoutAnimationEnabledExperimental) {
            UIManager.setLayoutAnimationEnabledExperimental(true);
        }
    }

	render() {
		return (
			<ThemeProvider uiTheme={uiTheme}>
                <Container>
				    <MainTabNavigator ref={(nav) => { this.navigator = nav; }} />
                </Container>
			</ThemeProvider>
		);
	}
}

export default App;