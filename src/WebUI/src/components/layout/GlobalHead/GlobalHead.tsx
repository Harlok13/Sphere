import {Head} from "components/shared/pages/main-page/Center/Head/Head";
import {Nav} from "components/shared/pages/main-page/Center/Head/Nav/Nav";
import {NavItems} from "components/shared/pages/main-page/Center/Head/Nav/NavItems/NavItems";
import {NavItem} from "components/shared/pages/main-page/Center/Head/Nav/NavItems/NavItem/NavItem";

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