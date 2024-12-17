"use client";
import React, { useEffect, useState } from "react";
import { Card, Button, Modal, Form, Input, Row, Col, message, Select } from "antd";
import axios from "axios";
import { ExclamationCircleOutlined } from "@ant-design/icons";
import { API_BASE_URL } from "../../../const";

const { confirm } = Modal;
const { Option } = Select;

interface Author {
  id: string;
  name: string;
}

interface Item {
  id: string;
  title: string;
  publicationDate: string;
  itemImageUrl: string;
  authors: Author[];
}

interface ItemFormValues {
  title: string;
  publicationDate: string;
  itemImageUrl?: string;
  authors: string[]; // массив идентификаторов авторов
}

const ItemsPage: React.FC = () => {
  const [items, setItems] = useState<Item[]>([]);
  const [authors, setAuthors] = useState<Author[]>([]);
  const [editingItem, setEditingItem] = useState<Item | null>(null);
  const [isEditModalVisible, setIsEditModalVisible] = useState(false);
  const [isAddModalVisible, setIsAddModalVisible] = useState(false);
  const [form] = Form.useForm();
  // const [isLoading, setIsLoading] = useState(false);

  // Получение списка элементов
  const fetchItems = async () => {
    // setIsLoading(true);
    try {
      const response = await axios.get<Item[]>(`${API_BASE_URL}/items`);
      setItems(response.data);
    } catch (error) {
      console.error("Ошибка при загрузке элементов", error);
    } finally {
      // setIsLoading(false);
    }
  };

  // Получение списка авторов
  const fetchAuthors = async () => {
    try {
      const response = await axios.get<Author[]>(`${API_BASE_URL}/authors`);
      setAuthors(response.data);
    } catch (error) {
      console.error("Ошибка при загрузке авторов", error);
    }
  };

  useEffect(() => {
    fetchItems();
    fetchAuthors();
  }, []);

  // Открытие модального окна для редактирования
  const openEditModal = (item: Item) => {
    setEditingItem(item);
    setIsEditModalVisible(true);
    form.setFieldsValue({
      title: item.title,
      publicationDate: item.publicationDate,
      itemImageUrl: item.itemImageUrl,
      authors: item.authors.map((author) => author.id),
    });
  };

  // Закрытие модального окна для редактирования
  const closeEditModal = () => {
    setEditingItem(null);
    setIsEditModalVisible(false);
    form.resetFields();
  };

  // Открытие модального окна для добавления
  const openAddModal = () => {
    setIsAddModalVisible(true);
    form.resetFields();
  };

  // Закрытие модального окна для добавления
  const closeAddModal = () => {
    setIsAddModalVisible(false);
    form.resetFields();
  };

  // Обновление элемента
  const updateItem = async (values: ItemFormValues) => {
    if (!editingItem) return;
    try {
      const updatedItem = {
        ...values,
        authorIds: values.authors, // Переводим список авторов в их идентификаторы
        categoryId: "6fd50949-61f4-4b66-ba7d-0ab75ec88fc0", // Стандартная категория
      };

      await axios.put(`${API_BASE_URL}/items/${editingItem.id}`, updatedItem);
      message.success("Элемент успешно обновлен");
      closeEditModal();
      fetchItems();
    } catch (error) {
      console.error("Ошибка при обновлении элемента", error);
    }
  };

  // Добавление нового элемента
  const addItem = async (values: ItemFormValues) => {
    try {
      const newItem = {
        ...values,
        authorIds: values.authors, // Переводим список авторов в их идентификаторы
        categoryId: "6fd50949-61f4-4b66-ba7d-0ab75ec88fc0", // Стандартная категория
      };

      await axios.post(`${API_BASE_URL}/items`, newItem);
      message.success("Элемент успешно добавлен");
      closeAddModal();
      fetchItems();
    } catch (error) {
      console.error("Ошибка при добавлении элемента", error);
    }
  };

  // Удаление элемента
  const confirmDelete = (id: string) => {
    confirm({
      title: "Вы уверены, что хотите удалить этот элемент?",
      icon: <ExclamationCircleOutlined />,
      okText: "Да",
      cancelText: "Нет",
      onOk: async () => {
        try {
          await axios.delete(`${API_BASE_URL}/items/${id}`);
          message.success("Элемент успешно удален");
          fetchItems();
        } catch (error) {
          console.error("Ошибка при удалении элемента", error);
        }
      },
    });
  };

  return (
    <div style={{ padding: "20px" }}>
      <Button type="primary" style={{ marginBottom: "20px" }} onClick={openAddModal}>
        Добавить элемент
      </Button>

      <Row gutter={[16, 16]}>
        {items.map((item) => (
          <Col key={item.id} xs={24} sm={12} md={8} lg={6}>
            <Card
              hoverable
              style={{
                maxWidth: "290px",
                margin: "0 auto",
                textAlign: "center",
              }}
              cover={
                <img
                  alt={item.title}
                  src={item.itemImageUrl || "https://i.pinimg.com/736x/98/57/30/9857307829310c7ec88d1ff8608f0619.jpg"}
                  style={{
                    height: '290px',
                    width: '100%',
                    objectFit: 'contain',
                  }}
                />
              }
            >
              <p>
                <b>Название:</b> {item.title}
              </p>
              <p>
                <b>Авторы:</b> {item.authors.map((author) => author.name).join(", ")}
              </p>
              <p>
                <b>Дата публикации:</b> {item.publicationDate || "Не указано"}
              </p>
              <Button type="link" onClick={() => openEditModal(item)}>
                Редактировать
              </Button>
              <Button type="link" danger onClick={() => confirmDelete(item.id)}>
                Удалить
              </Button>
            </Card>
          </Col>
        ))}
      </Row>

      {/* Модальное окно для редактирования */}
      <Modal
        title="Изменить элемент"
        visible={isEditModalVisible}
        onCancel={closeEditModal}
        onOk={() => form.submit()}
        okText="Сохранить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={updateItem}>
          <Form.Item
            name="title"
            label="Название"
            rules={[{ required: true, message: "Введите название" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="publicationDate"
            label="Дата публикации"
            rules={[{ required: true, message: "Введите дату публикации" }]}
          >
            <Input placeholder="Введите дату (например, 2024-12-15)" />
          </Form.Item>
          <Form.Item
            name="itemImageUrl"
            label="URL изображения"
            rules={[{ type: "url", message: "Введите корректный URL" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="authors"
            label="Авторы"
            rules={[{ required: true, message: "Выберите авторов" }]}
          >
            <Select mode="multiple" placeholder="Выберите авторов">
              {authors.map((author) => (
                <Option key={author.id} value={author.id}>
                  {author.name}
                </Option>
              ))}
            </Select>
          </Form.Item>
        </Form>
      </Modal>

      {/* Модальное окно для добавления */}
      <Modal
        title="Добавить элемент"
        visible={isAddModalVisible}
        onCancel={closeAddModal}
        onOk={() => form.submit()}
        okText="Добавить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={addItem}>
          <Form.Item
            name="title"
            label="Название"
            rules={[{ required: true, message: "Введите название" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="publicationDate"
            label="Дата публикации"
            rules={[{ required: true, message: "Введите дату публикации" }]}
          >
            <Input placeholder="Введите дату (например, 2024-12-15)" />
          </Form.Item>
          <Form.Item
            name="itemImageUrl"
            label="URL изображения"
            rules={[{ type: "url", message: "Введите корректный URL" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="authors"
            label="Авторы"
            rules={[{ required: true, message: "Выберите авторов" }]}
          >
            <Select mode="multiple" placeholder="Выберите авторов">
              {authors.map((author) => (
                <Option key={author.id} value={author.id}>
                  {author.name}
                </Option>
              ))}
            </Select>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default ItemsPage;
