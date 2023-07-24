import { useState, useEffect } from "react";
import UserView from "./UserView";
import { getPublicUser } from "../../services/usersService";
import { useParams } from "react-router-dom";


const UserPublicView = () => {
    const [user, setUser] = useState({
        id: 0,
        name: '',
        email: '',
        description: '',
        password: '',
        photo: '',
      });

      const params = useParams();
      const userId = params.userId;
    
      useEffect(() => {
        const fetchUser = async () => {
          const data = await getPublicUser(userId);
          setUser(data);
        };
    
        fetchUser();
      }, []);

      return <UserView user={user}/>;
}

export default UserPublicView;