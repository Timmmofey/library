'use client';

import { useState, useEffect } from 'react';
import { Form, Input, Button, Select, message } from 'antd';
import axios from 'axios';
import { NextPage } from 'next';
import { API_BASE_URL } from '../../../const';
import { useRouter } from 'next/navigation';
// import { NextApiRequest, NextApiResponse } from 'next';

// Определяем типы для библиотек
interface Library {
  id: string;
  name: string;
  address: string;
}

// Тип данных для регистрации читателя
interface RegisterData {
  email: string;
  passwordHash: string;
  fullName: string;
  libraryId: string;
}

// API запросы для получения библиотек и регистрации
const fetchLibraries = async (): Promise<Library[]> => {
  try {
    const response = await axios.get(`${API_BASE_URL}/Library`);
    return response.data;
  } catch (error) {
    console.error('Error fetching libraries:', error);
    throw error;
  }
};

const registerReader = async (data: RegisterData): Promise<void> => {
  try {
    await axios.post(`${API_BASE_URL}/api/Reader/addReader`, data);
  } catch (error) {
    console.error('Error registering reader:', error);
    throw error;
  }
};

// Страница регистрации
const RegisterPage: NextPage = () => {
  const [libraries, setLibraries] = useState<Library[]>([]);
  const [loading, setLoading] = useState<boolean>(false);

  const router = useRouter();

  useEffect(() => {
    const loadLibraries = async () => {
      try {
        const librariesData = await fetchLibraries();
        setLibraries(librariesData);
      } catch (error) {
        console.error('Ошибка при загрузке списка библиотек', error);
      }
    };

    loadLibraries();
  }, []);

  const onFinish = async (values: RegisterData) => {
    setLoading(true);
    try {
      await registerReader(values);
      message.success('Пользователь успешно зарегистрирован');
      router.push('/readerLogin');
    } catch (error) {
      console.error('Ошибка при регистрации пользователя', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: 'auto' }}>
      <h2>Регистрация читателя</h2>
      <Form onFinish={onFinish}>
        <Form.Item
          label="Email"
          name="email"
          rules={[{ required: true, message: 'Пожалуйста, введите email!' }]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Пароль"
          name="passwordHash"
          rules={[{ required: true, message: 'Пожалуйста, введите пароль!' }]}
        >
          <Input.Password />
        </Form.Item>

        <Form.Item
          label="Полное имя"
          name="fullName"
          rules={[{ required: true, message: 'Пожалуйста, введите полное имя!' }]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Библиотека"
          name="libraryId"
          rules={[{ required: true, message: 'Пожалуйста, выберите библиотеку!' }]}
        >
          <Select placeholder="Выберите библиотеку">
            {libraries.map((library) => (
              <Select.Option key={library.id} value={library.id}>
                {library.name} - {library.address}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>

        <Form.Item>
          <Button type="primary" htmlType="submit" loading={loading}>
            Зарегистрироваться
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
};

export default RegisterPage;
