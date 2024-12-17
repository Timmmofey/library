"use client"
import { Button } from 'antd';
import { useState } from 'react';
import { useAuthStore } from '../stores/authStore';
import { useReaderAuthStore } from '../stores/readerAuthStore';
import UserInfoModal from './UserInfoModal';
import ReaderInfoModal from './ReaderInfoModal'
import Link from 'next/link';

const Header = () => {
  const [modalVisible, setModalVisible] = useState(false);
  const user = useAuthStore((state) => state.user);
  const reader = useReaderAuthStore((state) => state.reader);
  

  return (
    <div style={{ display: 'flex', justifyContent: 'space-between', padding: '10px 20px', background: '#f0f2f5' }}>
      {/* <h2>Library System</h2> */}
      {user?.role == 0 && <Link href={"/admin"}>Главная</Link>}
      {user?.role == 1 && <Link href={"/director"}>Главная</Link>}
      {user?.role == 2 && <Link href={"/librarian"}>Главная</Link>}
      {reader?.id  && <Link href={"/reader"}>Главная</Link>}

      {(user || reader) && (
        <div>
          {user && <Button onClick={() => setModalVisible(true)}>{user.name } </Button>}
          {user && <UserInfoModal visible={modalVisible} onClose={() => setModalVisible(false)} /> }

          {reader && <Button onClick={() => setModalVisible(true)}>{reader?.fullName}</Button>}
          {reader && <ReaderInfoModal visible={modalVisible} onClose={() => setModalVisible(false)} />}
        </div>
      )}
    </div>
  );
};

export default Header;
