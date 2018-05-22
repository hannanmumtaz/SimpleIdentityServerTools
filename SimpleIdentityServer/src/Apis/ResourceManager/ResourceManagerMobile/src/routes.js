import { createDrawerNavigator } from 'react-navigation';
import Home from './components/home';
import Container from './components/common/container';

const AppNavigator = createDrawerNavigator({
	home: { screen: Home }
}, {
	initialRouteName: 'home',
	contentComponent: Container
})

export default AppNavigator;