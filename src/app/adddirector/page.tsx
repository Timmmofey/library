"use client"
import React, { useState, useEffect } from 'react';
import { Card, Select, Button, Input, Modal, Form, message } from 'antd';
import axios from 'axios';
import { FormInstance } from 'antd/lib/form';

import { API_BASE_URL } from '../../../const';


const { Option } = Select;

// Типы данных
interface Librarian {
  id: string;
  name: string;
  login: string;
  role: string;
  libraryId: string
}

interface Library {
  id: string;
  name: string;
}

// Тип для параметров поиска
interface SearchParams {
  name?: string;
  libraryName?: string;
}

// Установка глобальных настроек Axios
axios.defaults.withCredentials = true;

const LibrariansPage: React.FC = () => {
  const [directorNames, setDirectorNames] = useState<string[]>([]);
  const [librarians, setLibrarians] = useState<Librarian[]>([]);
  const [libraries, setLibraries] = useState<Library[]>([]);
  const [searchParams, setSearchParams] = useState<SearchParams>({});
  const [isAddModalOpen, setAddModalOpen] = useState(false);
  const [addDirectorForm] = Form.useForm<FormInstance>();
  const [isEditModalOpen, setEditModalOpen] = useState(false);
  const [selectedLibrarian, setSelectedLibrarian] = useState<Librarian | null>(null);
  const [editForm] = Form.useForm<FormInstance>();


  // Загрузка директоров для селекта
  useEffect(() => {
    axios
      .get<Librarian[]>(`${API_BASE_URL}/Librarians/search?role=1`, {
        withCredentials: true,
      })
      .then((res) => setDirectorNames(res.data.map((librarian) => librarian.name)))
      .catch(() => message.error("Не удалось загрузить список директоров."));
  }, []);

  // Загрузка библиотек при монтировании компонента
  useEffect(() => {
    axios
      .get<Library[]>(`${API_BASE_URL}/Library`, { withCredentials: true }) // Указываем withCredentials
      .then((res) => setLibraries(res.data))
      .catch(() => message.error('Не удалось загрузить библиотеки.'));
  }, []);

  // Обработка поиска библиотекарей
  const handleSearch = () => {
    axios
      .get<Librarian[]>(`${API_BASE_URL}/Librarians/search?role=1`, { 
        params: searchParams,
        withCredentials: true, // Указываем withCredentials
      })
      .then((res) => setLibrarians(res.data))
      .catch(() => message.error('Библиотекари не найдены.'));
  };

  // Обработка добавления директора
  const handleAddDirector = (values: { name: string; login: string; passwordHash: string; libraryId: string }) => {
    axios
      .post(
        `${API_BASE_URL}/Librarians/addDirector`, 
        values, 
        { withCredentials: true } // Указываем withCredentials
      )
      .then(() => {
        message.success('Директор успешно добавлен!');
        setAddModalOpen(false);
        addDirectorForm.resetFields();
      })
      .catch((err) =>
        message.error(err.response?.data || 'Ошибка при добавлении директора.')
      );
  };

  const handleEditClick = (id: string) => {
    axios
      .get<Librarian>(`${API_BASE_URL}/Librarians/${id}`, { withCredentials: true })
      .then((res) => {
        setSelectedLibrarian(res.data);
        setEditModalOpen(true);
        editForm.setFieldsValue(res.data); // Устанавливаем данные в форму
      })
      .catch(() => message.error('Не удалось загрузить данные библиотекаря.'));
  };
  
  const handleEditSubmit = (values: Partial<Librarian>) => {
    if (!selectedLibrarian) return;
  
    axios
      .put(`${API_BASE_URL}/Librarians/${selectedLibrarian.id}`, values, { withCredentials: true })
      .then(() => {
        message.success('Данные успешно обновлены!');
        setEditModalOpen(false);
        handleSearch(); // Обновляем список библиотекарей
      })
      .catch(() => message.error('Ошибка при обновлении данных.'));
  };

  const handleDelete = () => {
    if (!selectedLibrarian) return;
  
    Modal.confirm({
      title: 'Вы уверены, что хотите удалить этого библиотекаря?',
      content: `Удаление библиотекаря "${selectedLibrarian.name}" приведёт к потере всех связанных данных.`,
      okText: 'Удалить',
      okType: 'danger',
      cancelText: 'Отмена',
      onOk: () => {
        axios
          .delete(`${API_BASE_URL}/Librarians/${selectedLibrarian.id}`, {
            withCredentials: true,
          })
          .then(() => {
            message.success('Библиотекарь успешно удалён!');
            setEditModalOpen(false); // Закрываем модальное окно
            handleSearch(); // Обновляем список библиотекарей
          })
          .catch(() => message.error('Ошибка при удалении библиотекаря.'));
      },
    });
  };
  

  return (
    <>
    <div style={{ padding: '20px' }}>
      <h1 style={{fontSize: '30px'}}>Директоры</h1>

      {/* Кнопка добавления директора */}
      <Button
        type="primary"
        style={{ marginTop: '20px', marginBottom: '20px' }}
        onClick={() => setAddModalOpen(true)}
      >
        Добавить директора
      </Button>

      {/* Форма поиска */}
      <h1 style={{marginTop: '30px'}}>Форма поиска</h1>
      <div style={{ marginBottom: '20px' }}>
        <Select
          showSearch
          placeholder="Выберите имя библиотекаря"
          onChange={(value) => setSearchParams({ ...searchParams, name: value })}
          style={{ width: 200, marginRight: 10, marginBottom: '20px' }}
          allowClear
        >
          {/* Динамическое заполнение имен библиотекарей */}
          {directorNames.map((name, index) => (
            <Option key={index} value={name}>
              {name}
            </Option>
          ))}
        </Select>

        <Select
          showSearch
          placeholder="Выберите библиотеку"
          onChange={(value) =>
            setSearchParams({ ...searchParams, libraryName: value })
          }
          style={{ width: 200, marginRight: 10, marginBottom: '20px' }}
          allowClear
        >
          {/* Динамическое заполнение библиотек */}
          {libraries.map((library) => (
            <Option key={library.id} value={library.name}>
              {library.name}
            </Option>
          ))}
        </Select>

        <Button type="primary" onClick={handleSearch}>
          Поиск
        </Button>
      </div>

      {/* Карточки библиотекарей */}
      <div style={{ display: 'flex', flexWrap: 'wrap', gap: '20px' }}>
        {librarians.map((librarian) => (
          <Card
            key={librarian.id}
            title={librarian.name}
            style={{ width: 300 }}
            extra={
              <Button type="link" onClick={() => handleEditClick(librarian.id)}>
                Обновить
              </Button>
            }
          >
            <p>
              <strong>Логин:</strong> {librarian.login}
            </p>
            <p>
              <strong>Роль:</strong> Директор
            </p>
            <p>
              <strong>Библиотека: </strong> 
              {libraries.find((library) => library.id === librarian.libraryId)?.name || 'Неизвестная'}
            </p>
          </Card>
        ))}
      </div>

      {/* Модальное окно добавления директора */}
      <Modal
        title="Добавить директора"
        open={isAddModalOpen}
        onCancel={() => setAddModalOpen(false)}
        footer={null}
      >
        <Form
          form={addDirectorForm}
          layout="vertical"
          onFinish={handleAddDirector}
        >
          <Form.Item
            name="name"
            label="Имя"
            rules={[{ required: true, message: 'Введите имя директора' }]}
          >
            <Input placeholder="Введите имя директора" />
          </Form.Item>

          <Form.Item
            name="login"
            label="Логин"
            rules={[{ required: true, message: 'Введите логин' }]}
          >
            <Input placeholder="Введите логин" />
          </Form.Item>

          <Form.Item
            name="passwordHash"
            label="Пароль"
            rules={[{ required: true, message: 'Введите пароль' }]}
          >
            <Input.Password placeholder="Введите пароль" />
          </Form.Item>

          <Form.Item
            name="libraryId"
            label="Библиотека"
            rules={[{ required: true, message: 'Выберите библиотеку' }]}
          >
            <Select placeholder="Выберите библиотеку">
              {libraries.map((library) => (
                <Option key={library.id} value={library.id}>
                  {library.name}
                </Option>
              ))}
            </Select>
          </Form.Item>

          <Form.Item>
            <Button type="primary" htmlType="submit">
              Добавить директора
            </Button>
          </Form.Item>
        </Form>
      </Modal>
      <Modal
  title="Обновить данные библиотекаря"
  open={isEditModalOpen}
  onCancel={() => setEditModalOpen(false)}
  footer={null}
>
  <Form
    form={editForm}
    layout="vertical"
    onFinish={handleEditSubmit}
    initialValues={selectedLibrarian || {}}
  >
    <Form.Item
      name="id"
      label="ID"
    >
      <Input disabled />
    </Form.Item>

    <Form.Item
      name="name"
      label="Имя"
      rules={[{ required: true, message: 'Введите имя' }]}
    >
      <Input />
    </Form.Item>

    <Form.Item
      name="login"
      label="Логин"
      rules={[{ required: true, message: 'Введите логин' }]}
    >
      <Input />
    </Form.Item>

    <Form.Item
      name="passwordHash"
      label="Пароль"
      rules={[{ required: true, message: 'Введите пароль' }]}
    >
      <Input.Password />
    </Form.Item>

    <Form.Item
      name="libraryId"
      label="Библиотека"
      rules={[{ required: true, message: 'Выберите библиотеку' }]}
    >
      <Select placeholder="Выберите библиотеку">
        {libraries.map((library) => (
          <Option key={library.id} value={library.id}>
            {library.name}
          </Option>
        ))}
      </Select>
    </Form.Item>

    <Form.Item
      name="role"
      label="Роль"
    >
      <Select disabled>
        <Option value="1">Директор</Option>
      </Select>
    </Form.Item>

    <Form.Item>
      <Button type="primary" htmlType="submit">
        Сохранить
      </Button>
      <Button
        type="default"
        danger
        onClick={handleDelete}
        disabled={!selectedLibrarian}
        style={{marginLeft: "15px"}}
      >
        Удалить
      </Button>
    </Form.Item>
  </Form>
</Modal>

    </div>
    </>
  );
};

export default LibrariansPage;
