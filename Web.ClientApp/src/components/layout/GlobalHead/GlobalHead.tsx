import {Nav} from "../../../shared/pages/main-page/Center/Head/Nav/Nav";
import {NavItems} from "../../../shared/pages/main-page/Center/Head/Nav/NavItems/NavItems";
import {NavItem} from "../../../shared/pages/main-page/Center/Head/Nav/NavItems/NavItem/NavItem";
import {Head} from "../../../shared/pages/main-page/Center/Head/Head";

export const GlobalHead = () => {
    return (
        <Head>
            <Nav>
                <NavItems>
                    <NavItem/>
                </NavItems>
            </Nav>
        </Head>
    )
}