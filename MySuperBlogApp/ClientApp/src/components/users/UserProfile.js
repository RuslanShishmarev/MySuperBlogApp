import React, { useEffect, useState } from 'react';
import { exitFromProfile, getUser, updateUser } from '../../services/usersService';
import ImageComponent from '../ImageComponent';
import { NewsByUser } from '../news/News';
import ModalButton from '../ModalButton';
import UserProfileCreation from './UserProfileCreation';
import UserView from './UserView';

const UserProfile = () => {
  const [user, setUser] = useState({
    id: 0,
    name: '',
    email: '',
    description: '',
    password: '',
    photo: '',
  });

  useEffect(() => {
    const fetchUser = async () => {
      const data = await getUser();
      setUser(data);
    };

    fetchUser();
  }, []);

  const updateUserView = (newUser) => {
    setUser(newUser);
    updateUser(newUser);
  }

  return (
    <div>
    <div style={
      {
        display: 'flex',
        justifyContent: 'flex-end'
        }}>
        <ModalButton 
          btnName = {'Edit'}
          modalContent = {<UserProfileCreation user = {user} setAction={updateUserView}/>} 
          title = {'Редактирование профиля'}/>
        <button className="btn btn-secondary" onClick={() => exitFromProfile()}>Exit</button>
    </div>
    <UserView user={user}/>    
    </div>
  );
};

export default UserProfile;