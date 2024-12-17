'use client';
import { Modal, Button } from 'antd';
import { useReaderAuthStore } from '../stores/readerAuthStore';
import { useRouter } from 'next/navigation';
import Link from 'next/link';

interface Props {
  visible: boolean;
  onClose: () => void;
}

const ReaderInfoModal: React.FC<Props> = ({ visible, onClose }) => {
  const router = useRouter();
  const reader = useReaderAuthStore((state) => state.reader);
  const logout = useReaderAuthStore((state) => state.logout); // Подключаем метод logout

  const handleLogout = () => {
    logout(); // Вызываем метод logout
    onClose(); // Закрываем модальное окно
    router.push('/readerLogin');
  };

  const formatDate = (isoDate) => {
    const date = new Date(isoDate);
    return date.toLocaleDateString('ru', {
      day: '2-digit',
      month: 'long',
      year: 'numeric',
    });
  };

  if (!reader) return null;

  return (
    <Modal title="Профиль" visible={visible} onCancel={onClose} footer={null}>
      <p><b>Имя:</b> {reader.fullName}</p>
      <p><b>email:</b> {reader.email}</p>
      <p><b>Библиотека:</b> {reader.libraryName}</p>
      {reader.readerCategoryId && <p><b>Категория:</b> {reader.readerCategoryId}</p>}
      <p><b>Окончание подписки:</b> {formatDate(reader.subscriptionEndDate)}</p>
      {reader.readerCategoryId =="ceb525a5-8d68-4bd6-a962-e6e5f1a0ff3c" && reader.educationalInstitution && <p><b>Educational Institution:</b> {reader.educationalInstitution}</p>}
      {reader.readerCategoryId =="ceb525a5-8d68-4bd6-a962-e6e5f1a0ff3c" && reader.faculty && <p><b>faculty:</b> {reader.faculty}</p>}
      {reader.readerCategoryId =="ceb525a5-8d68-4bd6-a962-e6e5f1a0ff3c" && reader.course && <p><b>course:</b> {reader.course}</p>}
      {reader.readerCategoryId =="ceb525a5-8d68-4bd6-a962-e6e5f1a0ff3c" && reader.groupNumber && <p><b>Group number:</b> {reader.groupNumber}</p>}
      {reader.readerCategoryId =="ddb7a5f0-0e41-4509-b904-6abc77611f81" && reader.organization && <p><b>Organization:</b> {reader.organization}</p>}
      {reader.readerCategoryId =="ddb7a5f0-0e41-4509-b904-6abc77611f81" && reader.researchTopic && <p><b>Research Topic:</b> {reader.researchTopic}</p>}
      <Button type="primary" danger onClick={handleLogout} style={{ marginTop: '20px' }}>
        Выйти
      </Button>
      <Button style={{ marginLeft: '20px' }}>
        <Link href={"/subscription"}>Продлить подписку</Link>
      </Button>
    </Modal>
  );
};

export default ReaderInfoModal;
