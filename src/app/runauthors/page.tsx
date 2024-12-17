"use client";
import React, { useEffect, useState } from "react";
import { Card, Button, Modal, Form, Input, Row, Col, message } from "antd";
import axios from "axios";
import { ExclamationCircleOutlined } from "@ant-design/icons";
import { API_BASE_URL } from "../../../const";

const { confirm } = Modal;

interface Author {
  id: string;
  name: string;
  description: string;
}

const AuthorsPage: React.FC = () => {
  const [authors, setAuthors] = useState<Author[]>([]);
  const [editingAuthor, setEditingAuthor] = useState<Author | null>(null);
  const [isEditModalVisible, setIsEditModalVisible] = useState(false);
  const [isAddModalVisible, setIsAddModalVisible] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [form] = Form.useForm();

  // Получение всех авторов
  const fetchAuthors = async () => {
    setIsLoading(true);
    try {
      const response = await axios.get<Author[]>(`${API_BASE_URL}/authors`);
      setAuthors(response.data);
    } catch (error) {
      message.error("Ошибка при загрузке авторов");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchAuthors();
  }, []);

  // Открытие модального окна для изменения
  const openEditModal = (author: Author) => {
    setEditingAuthor(author);
    setIsEditModalVisible(true);
    form.setFieldsValue(author);
  };

  // Закрытие модального окна
  const closeEditModal = () => {
    setEditingAuthor(null);
    setIsEditModalVisible(false);
    form.resetFields();
  };

  // Обновление автора
  const updateAuthor = async (values: Author) => {
    if (!editingAuthor) return;
    try {
      await axios.put(`${API_BASE_URL}/authors/${editingAuthor.id}`, values);
      message.success("Автор успешно обновлен");
      closeEditModal();
      fetchAuthors();
    } catch (error) {
      message.error("Ошибка при обновлении автора");
    }
  };

  // Подтверждение удаления
  const confirmDelete = (id: string) => {
    confirm({
      title: "Вы уверены, что хотите удалить этого автора?",
      icon: <ExclamationCircleOutlined />,
      okText: "Да",
      cancelText: "Отмена",
      onOk: async () => {
        try {
          await axios.delete(`${API_BASE_URL}/authors/${id}`);
          message.success("Автор успешно удален");
          fetchAuthors();
        } catch (error) {
          message.error("Ошибка при удалении автора");
        }
      },
    });
  };

  // Открытие модального окна для добавления нового автора
  const openAddModal = () => {
    setIsAddModalVisible(true);
    form.resetFields();
  };

  // Закрытие модального окна для добавления
  const closeAddModal = () => {
    setIsAddModalVisible(false);
  };

  // Добавление нового автора
  const addAuthor = async (values: Author) => {
    try {
      const response = await axios.post(`${API_BASE_URL}/authors`, values);
      message.success("Автор успешно добавлен");
      closeAddModal();
      fetchAuthors();
    } catch (error) {
      message.error("Ошибка при добавлении автора");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      {/* Кнопка для добавления нового автора */}
      <Button type="primary" onClick={openAddModal} style={{ marginBottom: 20 }}>
        Добавить автора
      </Button>

      <Row gutter={[16, 16]}>
        {authors.map((author) => (
          <Col key={author.id} xs={24} sm={12} md={8} lg={6}>
            <Card
              title={author.name}
              bordered
              actions={[
                <Button
                  type="link"
                  onClick={() => openEditModal(author)}
                  key="edit"
                >
                  Изменить
                </Button>,
                <Button
                  type="link"
                  danger
                  onClick={() => confirmDelete(author.id)}
                  key="delete"
                >
                  Удалить
                </Button>,
              ]}
            >
              <p>{author.description}</p>
            </Card>
          </Col>
        ))}
      </Row>

      {/* Модальное окно для редактирования */}
      <Modal
        title="Изменение автора"
        visible={isEditModalVisible}
        onCancel={closeEditModal}
        onOk={() => form.submit()}
        okText="Сохранить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={updateAuthor}>
          <Form.Item
            name="name"
            label="Имя автора"
            rules={[{ required: true, message: "Введите имя автора" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="description"
            label="Описание"
            rules={[{ required: true, message: "Введите описание" }]}
          >
            <Input.TextArea />
          </Form.Item>
        </Form>
      </Modal>

      {/* Модальное окно для добавления нового автора */}
      <Modal
        title="Добавить нового автора"
        visible={isAddModalVisible}
        onCancel={closeAddModal}
        onOk={() => form.submit()}
        okText="Добавить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={addAuthor}>
          <Form.Item
            name="name"
            label="Имя автора"
            rules={[{ required: true, message: "Введите имя автора" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="description"
            label="Описание"
            rules={[{ required: true, message: "Введите описание" }]}
          >
            <Input.TextArea />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default AuthorsPage;
