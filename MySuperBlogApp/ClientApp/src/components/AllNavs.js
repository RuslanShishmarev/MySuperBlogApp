import { NavItem, NavLink } from "reactstrap";
import { ALLUSERS_URL, LOGIN_URL, PROFILE_URL, isUserOnline } from "../services/commonService";
import { Link } from 'react-router-dom';


const AllNavs = () => {
    let navs = [
        <NavItem>
            <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
        </NavItem>
        ];

    if (isUserOnline()){
        navs.push(
        <NavItem>
            <NavLink tag={Link} className="text-dark" to={ALLUSERS_URL}>Search</NavLink>
        </NavItem>);
        navs.push(
        <NavItem>
            <NavLink tag={Link} className="text-dark" to={PROFILE_URL}>My profile</NavLink>
        </NavItem>)
    }
    else {
        navs.push(
        <NavItem>
            <NavLink tag={Link} className="text-dark" to={LOGIN_URL}>Login</NavLink>
        </NavItem>)
    }

    return (
        <ul className="navbar-nav flex-grow">
              {navs}
        </ul>
    )
}

export default AllNavs;