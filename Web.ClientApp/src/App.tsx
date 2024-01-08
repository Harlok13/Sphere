import 'index.scss';
import {Router} from "routes/Router";
import {Notifications} from "components/shared/components/Notifications/Notifications";
import Wrapper from "components/shared/components/Wrapper/Wrapper";


export const App = () => {
    return (
        <Notifications>
            <Wrapper>
                <Router/>
            </Wrapper>
        </Notifications>
    );
}
