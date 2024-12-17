"use client"
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Card, Input, Radio, Button, Row, Col } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import Cookies from 'js-cookie';  // Импортируем библиотеку для работы с куками
import { API_BASE_URL } from '../../../const';

interface Book {
  id: string;
  inventoryNumber: string;
  itemName: string;
  readerName: string;
  librarianName: string;
  issueDate: string;
  dueDate: string;
  returnDate: string;
  lost: boolean;
  imageUrl: string;
}

const Books: React.FC = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [filteredBooks, setFilteredBooks] = useState<Book[]>([]);
  const [searchTermName, setSearchTermName] = useState<string>(''); // Поиск по названию
  const [searchTermInventory, setSearchTermInventory] = useState<string>(''); // Поиск по инвентарному номеру
  const [status, setStatus] = useState<number | null>(null); // 0 - не возвращены, 1 - возвращены, 2 - утеряны
  // const [isLost, setIsLost] = useState<number | null>(null); // Фильтрация по утерянным книгам

  useEffect(() => {
    // Извлекаем токен из куки
    const token = Cookies.get('some-cookie'); // Замените на ваш ключ куки

    if (token) {
      // Получаем данные с API, передаем токен в заголовке
      axios.get<Book[]>(`${API_BASE_URL}/api/Reader/books`, {
        headers: {
          Authorization: `Bearer ${token}`,  // Добавляем токен в заголовок
        },
        withCredentials: true,  // Если куки нужны для кросс-доменных запросов
      })
        .then(response => {
          setBooks(response.data);
          setFilteredBooks(response.data);
        })
        .catch(error => {
          console.error('Ошибка при получении данных:', error);
        });
    }
  }, []);

  const handleSearchChangeName = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTermName(e.target.value);
  };

  const handleSearchChangeInventory = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTermInventory(e.target.value);
  };

  const handleSearch = () => {
    let filtered = books;

    // Фильтрация по названию
    if (searchTermName) {
      filtered = filtered.filter((book) => 
        book.itemName.toLowerCase().includes(searchTermName.toLowerCase())
      );
    }

    // Фильтрация по инвентарному номеру
    if (searchTermInventory) {
      filtered = filtered.filter((book) => 
        book.inventoryNumber.includes(searchTermInventory)
      );
    }

    // Фильтрация по статусу (возвращена/не возвращена)
    if (status !== null) {
      filtered = filtered.filter((book) => {
        if (status === 0) return !book.returnDate;   // Не возвращены
        if (status === 1) return book.returnDate;    // Возвращены
        if (status === 2) return book.lost;          // Утеряны
        return true; // Если не фильтруем
      });
    }

    setFilteredBooks(filtered);
  };

  const handleStatusChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setStatus(parseInt(e.target.value));
  };

  return (
    <div>
      <h1 style={{ marginBottom: 20, fontWeight: 900 }}>История выдачи книг</h1>
      {/* Фильтры расположены в одном столбце */}
      <Row gutter={[16, 16]} style={{ marginBottom: 20 }}>
        <Col span={24}>
          <Input
            placeholder="Поиск по названию"
            value={searchTermName}
            onChange={handleSearchChangeName}
            onPressEnter={handleSearch}
            prefix={<SearchOutlined />}
          />
        </Col>
        <Col span={24}>
          <Input
            placeholder="Поиск по инвентарному номеру"
            value={searchTermInventory}
            onChange={handleSearchChangeInventory}
            onPressEnter={handleSearch}
            prefix={<SearchOutlined />}
          />
        </Col>
        <Col span={24}>
          <Radio.Group onChange={handleStatusChange} value={status} style={{ width: '100%' }}>
            <Radio.Button value={null}>Все</Radio.Button>
            <Radio.Button value={0}>Не возвращены</Radio.Button>
            <Radio.Button value={1}>Возвращены</Radio.Button>
            <Radio.Button value={2}>Утеряны</Radio.Button>
          </Radio.Group>
        </Col>
        <Col span={24}>
          <Button type="primary" onClick={handleSearch} block>
            Применить фильтры
          </Button>
        </Col>
      </Row>

      {/* Карточки, расположенные одна под одной */}
      <Row gutter={[16, 16]}>
        {filteredBooks.map((book) => (
          <Col key={book.id} span={24} style={{ marginBottom: '20px' }}>
            <Card
              hoverable
              style={{ maxWidth: '290px', margin: '0 auto' }} // Центрируем карточку
              cover={
                <img
                  alt={book.itemName}
                  src={book.imageUrl || 'https://i.pinimg.com/736x/98/57/30/9857307829310c7ec88d1ff8608f0619.jpg'}
                  style={{ height: '290px', width: '100%', objectFit: 'contain' }}
                />
              }
            >
              <p>Название: {book.itemName}</p>
              <p>Инвентарный номер: {book.inventoryNumber}</p>
              <p>Читатель: {book.readerName}</p>
              <p>Библиотекарь: {book.librarianName}</p>
              <p>Дата выдачи: {new Date(book.issueDate).toLocaleDateString()}</p>
              {/* <p>Дата возврата: {book.returnDate ? new Date(book.returnDate).toLocaleDateString() : 'Не возвращено'}</p> */}
              <p>Вернуть: {new Date(book.dueDate).toLocaleDateString()}</p>
              <p>Утеряна: {book.lost ? 'Да' : 'Нет'}</p>
            </Card>
          </Col>
        ))}
      </Row>
    </div>
  );
};

export default Books;
