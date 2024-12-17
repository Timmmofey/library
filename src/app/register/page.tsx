"use client"
import { useState, useEffect } from 'react';
import { Form, Input, Button, Select, message } from 'antd';
import axios from 'axios';
import { NextPage } from 'next';
import { NextApiRequest, NextApiResponse } from 'next';
import { API_BASE_URL } from '../../../const';
import { useRouter } from 'next/navigation';


// API запросы для получения библиотек и регистрации
const fetchLibraries = async () => {
  try {
    const response = await axios.get(`${API_BASE_URL}/Library`);
    return response.data;
  } catch (error) {
    console.error('Error fetching libraries:', error);
    throw error;
  }
};

const registerReader = async (data: {
  email: string;
  passwordHash: string;
  fullName: string;
  libraryId: string;
}) => {
  try {
    const response = await axios.post(`${API_BASE_URL}/api/Reader/addReader`, data);
    return response.data;
  } catch (error) {
    console.error('Error registering reader:', error);
    throw error;
  }
};

// Страница регистрации
const RegisterPage: NextPage = () => {
  const [libraries, setLibraries] = useState([]);
  const [loading, setLoading] = useState(false);

  const router = useRouter()

  useEffect(() => {
    // Загружаем список библиотек при монтировании компонента
    const loadLibraries = async () => {
      try {
        const librariesData = await fetchLibraries();
        setLibraries(librariesData);
      } catch (error) {
        message.error('Ошибка при загрузке списка библиотек');
      }
    };

    loadLibraries();
  }, []);

  const onFinish = async (values: any) => {
    setLoading(true);
    try {
      await registerReader({
        email: values.email,
        passwordHash: values.passwordHash,
        fullName: values.fullName,
        libraryId: values.libraryId,
      });
      message.success('Пользователь успешно зарегистрирован');
      router.push("/readerLogin")
    } catch (error) {
      message.error('Ошибка при регистрации пользователя');
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
            {libraries.map((library: any) => (
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

// API для получения списка библиотек
export async function getLibrariesHandler(req: NextApiRequest, res: NextApiResponse) {
  const libraries = [
    {
      id: 'd1bbf679-bedd-4d03-b0cf-97ae5cd8b315',
      name: 'Библиотека №1',
      address: 'Москва, ул. Пушкина, д. 10',
      description: 'Описание библиотеки №1',
    },
    {
      id: 'a4f3e4a0-e2be-4a3d-9fd9-16dbf1fd55b7',
      name: 'Библиотека №2',
      address: 'Москва, ул. Лермонтова, д. 5',
      description: 'Описание библиотеки №2',
    },
  ];

  if (req.method === 'GET') {
    res.status(200).json(libraries);
  } else {
    res.status(405).json({ message: 'Method Not Allowed' });
  }
}

// API для регистрации пользователя
export async function addReaderHandler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method === 'POST') {
    const { email, passwordHash, fullName, libraryId } = req.body;

    if (!email || !passwordHash || !fullName || !libraryId) {
      return res.status(400).json({ message: 'Все поля обязательны для заполнения' });
    }

    const newReader = {
      id: `reader-${Math.random()}`,
      email,
      passwordHash,
      fullName,
      libraryId,
    };

    res.status(201).json(newReader);
  } else {
    res.status(405).json({ message: 'Method Not Allowed' });
  }
}

export default RegisterPage;
