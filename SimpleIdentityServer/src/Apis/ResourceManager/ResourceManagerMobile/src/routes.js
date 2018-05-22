import { createStackNavigator } from 'react-navigation';
import Home from './App/Home';

const AppNavigator = createStackNavigator({
	home: { screen: Home }
}, {
	headerMode: 'none',
    initialRouteName: 'home'
})

export default AppNavigator;