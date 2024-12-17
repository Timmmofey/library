'use client';
import { Modal, Button } from 'antd';
import { useAuthStore } from '../stores/authStore';
import { useRouter } from 'next/navigation';

interface Props {
  visible: boolean;
  onClose: () => void;
}

const UserInfoModal: React.FC<Props> = ({ visible, onClose }) => {
  const router = useRouter();
  const user = useAuthStore((state) => state.user);
  const logout = useAuthStore((state) => state.logout); // Подключаем метод logout

  const handleLogout = () => {
    logout(); // Вызываем метод logout
    onClose(); // Закрываем модальное окно
    router.push('/readerLogin');
  };

  if (!user) return null;

  const RoleMapping: Record<number, string> = {
    0: 'Админ',
    1: 'Директор',
    2: 'Библиотекарь',
  };

  const getRoleName = (role: number): string => RoleMapping[role] || 'Неизвестная роль';

  return (
    <Modal title="Профиль" visible={visible} onCancel={onClose} footer={null}>
      <p><b>Имя:</b> {user.name}</p>
      <p><b>Логин:</b> {user.login}</p>
      {user.libraryName && <p><b>Библиотека:</b> {user.libraryName}</p>}
      {user.readingRoomName && <p><b>Читальный зал:</b> {user.readingRoomName}</p>}
      <p><b>Роль:</b> {getRoleName(user.role)}</p>
      <Button type="primary" danger onClick={handleLogout} style={{ marginTop: '20px' }}>
        Выйти
      </Button>
    </Modal>
  );
};

export default UserInfoModal;
