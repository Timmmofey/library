"use client";
import React, { useEffect, useState } from "react";
import { Card, Input, Row, Col, Button, Select } from "antd";
import { SearchOutlined } from "@ant-design/icons";
import axios from "axios";
import { API_BASE_URL } from "../../../const";

const { Option } = Select;

interface Book {
  itemCopyId: string;
  itemId: string;
  title: string;
  authors: string[];
  inventoryNumber: string;
  loanable: boolean;
  libraryName: string | null;
  loaned: boolean;
  lost: boolean;
  dateReceived: string | null;
  dateWithdrawn: string | null;
  publications: string;
  imageUrl: string | null;
}

const BooksPage: React.FC = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [filteredBooks, setFilteredBooks] = useState<Book[]>([]);
  const [searchTitle, setSearchTitle] = useState<string>("");
  const [searchInventoryNumber, setSearchInventoryNumber] = useState<string>("");
  const [searchAuthor, setSearchAuthor] = useState<string>("");
  const [libraryFilter, setLibraryFilter] = useState<string | null>(null);
  const [libraries, setLibraries] = useState<string[]>([]);

  // Получение данных из API
  useEffect(() => {
    axios
      .get<Book[]>(`${API_BASE_URL}/ItemCopies/getalldtos`)
      .then((response) => {
        setBooks(response.data);
        setFilteredBooks(response.data);

        // Получение списка уникальных библиотек
        const uniqueLibraries = Array.from(
          new Set(response.data.map((book) => book.libraryName || "Неизвестно"))
        );
        setLibraries(uniqueLibraries);
      })
      .catch((error) => {
        console.error("Ошибка при загрузке данных:", error);
      });
  }, []);

  // Функция для применения фильтров
  const handleSearch = () => {
    const filtered = books.filter((book) => {
      const titleMatches = book.title
        .toLowerCase()
        .includes(searchTitle.toLowerCase());
      const inventoryMatches = book.inventoryNumber
        .toLowerCase()
        .includes(searchInventoryNumber.toLowerCase());
      const authorMatches = book.authors.some((author) =>
        author.toLowerCase().includes(searchAuthor.toLowerCase())
      );
      const libraryMatches =
        !libraryFilter || book.libraryName === libraryFilter;

      return titleMatches && inventoryMatches && authorMatches && libraryMatches;
    });
    setFilteredBooks(filtered);
  };

  return (
    <div style={{ padding: "20px" }}>
      {/* Фильтры */}
      <Row gutter={[16, 16]} style={{ marginBottom: "20px" }}>
        {/* Поиск по названию */}
        <Col span={24}>
          <Input
            placeholder="Поиск по названию"
            value={searchTitle}
            onChange={(e) => setSearchTitle(e.target.value)}
            onPressEnter={handleSearch}
            prefix={<SearchOutlined />}
          />
        </Col>
        {/* Поиск по инвентарному номеру */}
        <Col span={24}>
          <Input
            placeholder="Поиск по инвентарному номеру"
            value={searchInventoryNumber}
            onChange={(e) => setSearchInventoryNumber(e.target.value)}
            onPressEnter={handleSearch}
            prefix={<SearchOutlined />}
          />
        </Col>
        {/* Поиск по автору */}
        <Col span={24}>
          <Input
            placeholder="Поиск по автору"
            value={searchAuthor}
            onChange={(e) => setSearchAuthor(e.target.value)}
            onPressEnter={handleSearch}
            prefix={<SearchOutlined />}
          />
        </Col>
        {/* Фильтр по библиотекам */}
        <Col span={24}>
          <Select
            placeholder="Выберите библиотеку"
            value={libraryFilter}
            onChange={(value) => setLibraryFilter(value)}
            allowClear
            style={{ width: "100%" }}
          >
            {libraries.map((library) => (
              <Option key={library} value={library}>
                {library}
              </Option>
            ))}
          </Select>
        </Col>
      </Row>
      {/* Кнопка для применения фильтров */}
      <Row gutter={[16, 16]} style={{ marginBottom: "20px" }}>
        <Col span={24}>
          <Button type="primary" onClick={handleSearch} block>
            Применить фильтры
          </Button>
        </Col>
      </Row>
      {/* Карточки книг */}
      <Row gutter={[16, 16]}>
        {filteredBooks.map((book) => (
          <Col key={book.itemCopyId} span={24} style={{ marginBottom: "20px" }}>
            <Card
              hoverable
              style={{
                maxWidth: "290px",
                margin: "0 auto",
                textAlign: "center",
              }}
              cover={
                <img
                  alt={book.title}
                  src={
                    book.imageUrl ||
                    "https://i.pinimg.com/736x/98/57/30/9857307829310c7ec88d1ff8608f0619.jpg"
                  }
                  style={{
                    height: '290px', width: '100%', objectFit: 'contain'
                  }}
                />
              }
            >
              <p>
                <b>Название:</b> {book.title}
              </p>
              <p>
                <b>Инвентарный номер:</b> {book.inventoryNumber}
              </p>
              <p>
                <b>Авторы:</b> {book.authors.join(", ")}
              </p>
              <p>
                <b>Библиотека:</b> {book.libraryName || "Неизвестно"}
              </p>
              <p>
                <b>Дата получения:</b>{" "}
                {book.dateReceived
                  ? new Date(book.dateReceived).toLocaleDateString()
                  : "Не указано"}
              </p>
              <p>
                <b>Дата списания:</b>{" "}
                {book.dateWithdrawn
                  ? new Date(book.dateWithdrawn).toLocaleDateString()
                  : "Не указано"}
              </p>
              <p>
                <b>Год публикации:</b> {book.publications}
              </p>
            </Card>
          </Col>
        ))}
      </Row>
    </div>
  );
};

export default BooksPage;
